
using Microsoft.EntityFrameworkCore;
using SistemaRestauranteApi.Data;
using SistemaRestauranteApi.DTOs;
using SistemaRestauranteApi.Interfaces;
using SistemaRestauranteApi.Models;

namespace SistemaRestauranteApi.Services
{
    public class ClienteService : IClienteService
    {
        private readonly AppDbContext _context;

        public ClienteService(AppDbContext context)
        {
            this._context = context;
        }

         public async Task<List<ClienteResponseDto>> ObtenerTodosAsync()
        {
            return await _context.Clientes.Select(c => new ClienteResponseDto()
                                            {
                                                Id = c.Id,
                                                Nombre = c.Nombre,
                                                Apellido = c.Apellido,
                                                Telefono = c.Telefono
                                            })
                                            .ToListAsync();
        }

        public async Task<ClienteResponseDto?> BuscarClientePorIdAsync(int id)
        {
            var existe = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);

            if (existe is null)
            {
                return null;
            }

            return new ClienteResponseDto()
            {
                Id = existe.Id,
                Nombre = existe.Nombre,
                Apellido = existe.Apellido,
                Telefono = existe.Telefono
            };
        }

        public async Task<ClienteResponseDto?> CrearClienteAsync(ClienteRequestDto cliente)
        {
            var nuevoCliente = new Cliente()
            {
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Telefono = cliente.Telefono
            };

            _context.Clientes.Add(nuevoCliente);
            await _context.SaveChangesAsync();

            return await BuscarClientePorIdAsync(nuevoCliente.Id);
        }

        public async Task<ClienteResponseDto?> ActualizarCliente(int id, ClienteRequestDto cliente)
        {
            var existe = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);

            if (existe is null)
            {
                return null;
            }

            existe.Nombre = cliente.Nombre;
            existe.Apellido = cliente.Apellido;
            existe.Telefono = cliente.Telefono;

            await _context.SaveChangesAsync();

            return await BuscarClientePorIdAsync(existe.Id);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var existe = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);

            if (existe is null)
            {
                return false;
            }

            var tienePedidos = await _context.Pedidos.AnyAsync(p => p.ClienteId == id);

            if (tienePedidos)
            {
                return false;
            }

            _context.Clientes.Remove(existe);
            await _context.SaveChangesAsync();

            return true;
        }

        
    }
}
