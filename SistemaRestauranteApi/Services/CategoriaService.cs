
using Microsoft.EntityFrameworkCore;
using SistemaRestauranteApi.Data;
using SistemaRestauranteApi.DTOs;
using SistemaRestauranteApi.Interfaces;
using SistemaRestauranteApi.Models;

namespace SistemaRestauranteApi.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly AppDbContext _context;

        public CategoriaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaResponseDto>> GetAllAsync()
        {
            return await _context.Set<Categoria>()
                .Select(c => new CategoriaResponseDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Estado = c.Estado
                })
                .ToListAsync();
        }

        public async Task<CategoriaResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Set<Categoria>()
                .Where(c => c.Id == id)
                .Select(c => new CategoriaResponseDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Estado = c.Estado
                })
                .FirstOrDefaultAsync();
        }
    }
}
