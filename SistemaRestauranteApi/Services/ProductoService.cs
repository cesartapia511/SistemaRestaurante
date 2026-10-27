
using Microsoft.EntityFrameworkCore;
using SistemaRestauranteApi.Data;
using SistemaRestauranteApi.DTOs;
using SistemaRestauranteApi.Interfaces;
using SistemaRestauranteApi.Models;

namespace SistemaRestauranteApi.Services
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<List<ProductoResponseDto>> ObtenerTodosAsync()
        {
            return await _context.Productos.Include(p => p.Categoria)
                                            .Select(p => new ProductoResponseDto()
                                            {
                                                Id = p.Id,
                                                Nombre = p.Nombre,
                                                Descripcion = p.Descripcion,
                                                ImagenUrl = p.ImagenUrl,
                                                Precio = p.Precio,
                                                Disponible = p.Disponible,
                                                CategoriaId = p.CategoriaId,
                                                NombreCategoria = p.Categoria.Nombre
                                            }).ToListAsync();
        }

        public async Task<ProductoResponseDto?> BuscarProductoPorIdAsync(int id)
        {
            var existe = await _context.Productos.Include(p => p.Categoria)
                                                .FirstOrDefaultAsync(p => p.Id == id);
            if (existe is null)
            {
                return null;
            }

            return new ProductoResponseDto()
            {
                Id = existe.Id,
                Nombre = existe.Nombre,
                Descripcion = existe.Descripcion,
                ImagenUrl = existe.ImagenUrl,
                Precio = existe.Precio,
                Disponible = existe.Disponible,
                CategoriaId = existe.CategoriaId,
                NombreCategoria = existe.Categoria.Nombre
            };
        }

        public async Task<ProductoResponseDto?> CrearProductoAsync(ProductoRequestDto productoRequest)
        {
            var categoria = await _context.Categorias.FindAsync(productoRequest.CategoriaId);

            if (categoria is null)
            {
                return null;
            }

            var producto = new Producto()
            {
                Nombre = productoRequest.Nombre,
                Descripcion = productoRequest.Descripcion,
                ImagenUrl = productoRequest.ImagenUrl,
                Precio = productoRequest.Precio,
                Disponible = true,
                CategoriaId = productoRequest.CategoriaId
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return await this.BuscarProductoPorIdAsync(producto.Id);
        }

        public async Task<ProductoResponseDto?> ActualizarProducto(int id, ProductoRequestDto productoRequest)
        {
            var existe = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);

            if (existe is null)
            {
                return null;
            }

            var categoria = await _context.Categorias.FindAsync(productoRequest.CategoriaId);

            if (categoria is null)
            {
                return null;
            }

            existe.Nombre = productoRequest.Nombre;
            existe.Descripcion = productoRequest.Descripcion;
            existe.ImagenUrl = productoRequest.ImagenUrl;
            existe.Precio = productoRequest.Precio;
            existe.CategoriaId = productoRequest.CategoriaId;

            await _context.SaveChangesAsync();

            return await this.BuscarProductoPorIdAsync(existe.Id);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var existe = await _context.Productos.FindAsync(id);

            if (existe is null)
            {
                return false;
            }

            var tieneDetalles = await _context.DetallesPedidos.AnyAsync(d => d.ProductoId == id);

            if (tieneDetalles)
            {
                return false;
            }

            _context.Productos.Remove(existe);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
