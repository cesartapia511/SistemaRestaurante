
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
            _context = context;
        }

        public async Task<IEnumerable<ProductoResponseDto>> GetAllAsync()
        {
            return await _context.Set<Producto>()
                .Select(p => new ProductoResponseDto
                {
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Disponible = p.Disponible,
                    CategoriaId = p.CategoriaId
                })
                .ToListAsync();
        }

        public async Task<ProductoResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Set<Producto>()
                .Where(p => p.Id == id)
                .Select(p => new ProductoResponseDto
                {
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Disponible = p.Disponible,
                    CategoriaId = p.CategoriaId
                })
                .FirstOrDefaultAsync();
        }
    }
}
