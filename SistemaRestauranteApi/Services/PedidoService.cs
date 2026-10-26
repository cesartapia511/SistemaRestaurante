using Microsoft.EntityFrameworkCore;
using SistemaRestauranteApi.Data;
using SistemaRestauranteApi.DTOs;
using SistemaRestauranteApi.Interfaces;
using SistemaRestauranteApi.Models;
public class PedidoService : IPedidoService
{
    private readonly AppDbContext _context;
    public PedidoService(AppDbContext context)
    {
        this._context = context;
    }
    
    public async Task<PedidoResponseDto?> ActualizarPedidoAsync(int id, PedidoRequestDto pedidoRequest)
    {
        var existe = await _context.Pedidos.Include(p => p.Detalles)
                                            .FirstOrDefaultAsync(p => p.Id == id);

        if (existe is null)
        {
            return null;
        }
        if (existe.Estado == "Servido" || existe.Estado == "Cancelado")
        {
            return null;
        }

        var mesa = await _context.Mesas.FindAsync(pedidoRequest.MesaId);
        var cliente = await _context.Clientes.FindAsync(pedidoRequest.ClienteId);

        if (mesa is null || cliente is null)
        {
            return null;
        }

        if (pedidoRequest.Detalles is null || pedidoRequest.Detalles.Count == 0)
        {
            return null;
        }

        var nuevosDetalles = new List<DetallePedido>();
        decimal total = 0;

        foreach (var item in pedidoRequest.Detalles)
        {
            if (item is null || item.Cantidad <= 0)
            {
                return null;
            }

            var producto = await _context.Productos.FindAsync(item.ProductoId);

            if (producto is null || producto.Disponible == false)
            {
                return null;
            }

            var detalleAnterior = existe.Detalles
                .FirstOrDefault(d => d.ProductoId == item.ProductoId);

            decimal precio = producto.Precio;

            if (detalleAnterior is not null)
            {
                precio = detalleAnterior.PrecioUnitario;
            }

            if (precio <= 0)
            {
                return null;
            }

            var detalle = new DetallePedido()
            {
                ProductoId = producto.Id,
                Cantidad = item.Cantidad,
                PrecioUnitario = precio,
                Subtotal = item.Cantidad * precio
            };

            nuevosDetalles.Add(detalle);
            total = total + detalle.Subtotal;
        }

        _context.DetallesPedidos.RemoveRange(existe.Detalles);

        existe.MesaId = pedidoRequest.MesaId;
        existe.ClienteId = pedidoRequest.ClienteId;
        existe.MetodoPago = pedidoRequest.MetodoPago;
        existe.Detalles = nuevosDetalles;
        existe.Total = total;

        await _context.SaveChangesAsync();

        return await this.BuscarPedidoPorIdAsync(existe.Id);
    }

    public async Task<PedidoResponseDto?> BuscarPedidoPorIdAsync(int id)
    {
        var existe = await _context.Pedidos.Include(p => p.Mesa)
                                            .Include(p => p.Cliente)
                                            .Include(p => p.Detalles)
                                                .ThenInclude(d => d.Producto)
                                            .FirstOrDefaultAsync(p => p.Id == id);

        if (existe is null)
        {
            return null;
        }

        return new PedidoResponseDto()
        {
            Id = existe.Id,
            Fecha = DateOnly.FromDateTime(existe.Fecha),
            Estado = existe.Estado,
            Total = existe.Total,
            MetodoPago = existe.MetodoPago,

            MesaId = existe.MesaId,
            NumeroMesa = existe.Mesa.Numero,

            ClienteId = existe.ClienteId,
            NombreCliente = existe.Cliente.Nombre + " " + existe.Cliente.Apellido,

            Detalles = existe.Detalles.Select(d => new DetallePedidoResponseDto()
                                        {
                                            Id = d.Id,
                                            ProductoId = d.ProductoId,
                                            NombreProducto = d.Producto.Nombre,
                                            Cantidad = d.Cantidad,
                                            PrecioUnitario = d.PrecioUnitario,
                                            Subtotal = d.Subtotal
                                        }).ToList()
        };
    }

    public async Task<List<PedidoResponseDto>> BuscarPorFechaAsync(DateTime fecha)
    {
        var inicio = new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0, DateTimeKind.Utc);

        var fin = inicio.AddDays(1);

        return await _context.Pedidos.Include(p => p.Mesa)
                                    .Include(p => p.Cliente)
                                    .Include(p => p.Detalles)
                                        .ThenInclude(d => d.Producto)
                                    .Where(p => p.Fecha >= inicio && p.Fecha < fin)
                                    .Select(p => new PedidoResponseDto()
                                    {
                                        Id = p.Id,
                                        Fecha = DateOnly.FromDateTime(p.Fecha),
                                        Estado = p.Estado,
                                        Total = p.Total,
                                        MetodoPago = p.MetodoPago,

                                        MesaId = p.MesaId,
                                        NumeroMesa = p.Mesa.Numero,

                                        ClienteId = p.ClienteId,
                                        NombreCliente = p.Cliente.Nombre + " " + p.Cliente.Apellido,

                                        Detalles = p.Detalles
                                            .Select(d => new DetallePedidoResponseDto()
                                            {
                                                Id = d.Id,
                                                ProductoId = d.ProductoId,
                                                NombreProducto = d.Producto.Nombre,
                                                Cantidad = d.Cantidad,
                                                PrecioUnitario = d.PrecioUnitario,
                                                Subtotal = d.Subtotal
                                            }).ToList()
                                    }).ToListAsync();
    }

    public async Task<List<PedidoResponseDto>> BuscarPorMesaAsync(int mesaId)
    {
        return await _context.Pedidos.Include(p => p.Mesa)
                                    .Include(p => p.Cliente)
                                    .Include(p => p.Detalles)
                                        .ThenInclude(d => d.Producto)
                                    .Where(p => p.MesaId == mesaId)
                                    .Select(p => new PedidoResponseDto()
                                    {
                                        Id = p.Id,
                                        Fecha = DateOnly.FromDateTime(p.Fecha),
                                        Estado = p.Estado,
                                        Total = p.Total,
                                        MetodoPago = p.MetodoPago,

                                        MesaId = p.MesaId,
                                        NumeroMesa = p.Mesa.Numero,

                                        ClienteId = p.ClienteId,
                                        NombreCliente = p.Cliente.Nombre + " " + p.Cliente.Apellido,

                                        Detalles = p.Detalles
                                            .Select(d => new DetallePedidoResponseDto()
                                            {
                                                Id = d.Id,
                                                ProductoId = d.ProductoId,
                                                NombreProducto = d.Producto.Nombre,
                                                Cantidad = d.Cantidad,
                                                PrecioUnitario = d.PrecioUnitario,
                                                Subtotal = d.Subtotal
                                            }).ToList()
                                    }).ToListAsync();
    }

    public async Task<List<PedidoResponseDto>> BuscarPorMetodoPagoAsync(string metodoPago)
    {
        return await _context.Pedidos.Include(p => p.Mesa)
                                    .Include(p => p.Cliente)
                                    .Include(p => p.Detalles)
                                        .ThenInclude(d => d.Producto)
                                    .Where(p => p.MetodoPago.ToLower() == metodoPago.ToLower())
                                    .Select(p => new PedidoResponseDto()
                                    {
                                        Id = p.Id,
                                        Fecha = DateOnly.FromDateTime(p.Fecha),
                                        Estado = p.Estado,
                                        Total = p.Total,
                                        MetodoPago = p.MetodoPago,

                                        MesaId = p.MesaId,
                                        NumeroMesa = p.Mesa.Numero,

                                        ClienteId = p.ClienteId,
                                        NombreCliente = p.Cliente.Nombre + " " + p.Cliente.Apellido,

                                        Detalles = p.Detalles
                                            .Select(d => new DetallePedidoResponseDto()
                                            {
                                                Id = d.Id,
                                                ProductoId = d.ProductoId,
                                                NombreProducto = d.Producto.Nombre,
                                                Cantidad = d.Cantidad,
                                                PrecioUnitario = d.PrecioUnitario,
                                                Subtotal = d.Subtotal
                                            }).ToList()
                                    }).ToListAsync();
    }

    public async Task<PedidoResponseDto?> CambiarEstadoAsync(int id, PedidoEstadoRequestDto pedidoEstado)
    {
        var existe = await _context.Pedidos.FindAsync(id);

        if (existe is null)
        {
            return null;
        }

        string nuevoEstado = pedidoEstado.Estado;

        if (nuevoEstado != "Pendiente"
            && nuevoEstado != "En preparacion"
            && nuevoEstado != "Servido"
            && nuevoEstado != "Cancelado")
        {
            return null;
        }

        if (existe.Estado == nuevoEstado)
        {
            return await this.BuscarPedidoPorIdAsync(id);
        }

        bool cambioPermitido = false;

        if (existe.Estado == "Pendiente")
        {
            cambioPermitido = nuevoEstado == "En preparacion" || nuevoEstado == "Cancelado";
        }
        else if (existe.Estado == "En preparacion")
        {
            cambioPermitido = nuevoEstado == "Servido" || nuevoEstado == "Cancelado";
        }

        if (cambioPermitido == false)
        {
            return null;
        }

        existe.Estado = nuevoEstado;

        await _context.SaveChangesAsync();

        return await BuscarPedidoPorIdAsync(id);
    }

    public async Task<PedidoResponseDto?> CrearPedidoAsync(PedidoRequestDto pedidoRequest)
    {
        var mesa = await _context.Mesas.FindAsync(pedidoRequest.MesaId);
        var cliente = await _context.Clientes.FindAsync(pedidoRequest.ClienteId);

        if (mesa is null || cliente is null)
        {
            return null;
        }

        if (pedidoRequest.Detalles is null || pedidoRequest.Detalles.Count == 0)
        {
            return null;
        }

        var pedido = new Pedido()
        {
            Fecha = DateTime.UtcNow,
            Estado = "Pendiente",
            MesaId = pedidoRequest.MesaId,
            ClienteId = pedidoRequest.ClienteId,
            MetodoPago = pedidoRequest.MetodoPago,
            Total = 0
        };

        foreach (var item in pedidoRequest.Detalles)
        {
            if (item is null || item.Cantidad <= 0)
            {
                return null;
            }

            var producto = await _context.Productos.FindAsync(item.ProductoId);

            if (producto is null || producto.Disponible == false
                || producto.Precio <= 0)
            {
                return null;
            }

            var detalle = new DetallePedido()
            {
                ProductoId = producto.Id,
                Cantidad = item.Cantidad,
                PrecioUnitario = producto.Precio,
                Subtotal = item.Cantidad * producto.Precio
            };

            pedido.Detalles.Add(detalle);
            pedido.Total = pedido.Total + detalle.Subtotal;
        }

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return await this.BuscarPedidoPorIdAsync(pedido.Id);
    }

    public async Task<bool> EliminarPedidoAsync(int id)
    {
        var existe = await _context.Pedidos.Include(p => p.Detalles)
                                            .FirstOrDefaultAsync(p => p.Id == id);

        if (existe is null)
        {
            return false;
        }

        _context.DetallesPedidos.RemoveRange(existe.Detalles);
        _context.Pedidos.Remove(existe);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<PedidoResponseDto>> ObtenerTodosAsync()
    {
        return await _context.Pedidos.Include(p => p.Mesa)
                                    .Include(p => p.Cliente)
                                    .Include(p => p.Detalles)
                                        .ThenInclude(d => d.Producto)
                                    .Select(p => new PedidoResponseDto()
                                    {
                                        Id = p.Id,
                                        Fecha = DateOnly.FromDateTime(p.Fecha),
                                        Estado = p.Estado,
                                        Total = p.Total,
                                        MetodoPago = p.MetodoPago,

                                        MesaId = p.MesaId,
                                        NumeroMesa = p.Mesa.Numero,

                                        ClienteId = p.ClienteId,
                                        NombreCliente = p.Cliente.Nombre + " " + p.Cliente.Apellido,

                                        Detalles = p.Detalles
                                            .Select(d => new DetallePedidoResponseDto()
                                            {
                                                Id = d.Id,
                                                ProductoId = d.ProductoId,
                                                NombreProducto = d.Producto.Nombre,
                                                Cantidad = d.Cantidad,
                                                PrecioUnitario = d.PrecioUnitario,
                                                Subtotal = d.Subtotal
                                            }).ToList()
                                    }).ToListAsync();
    }
}