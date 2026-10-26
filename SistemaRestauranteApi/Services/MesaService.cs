
using Microsoft.EntityFrameworkCore;
using SistemaRestauranteApi.Data;
using SistemaRestauranteApi.DTOs;
using SistemaRestauranteApi.Interfaces;
using SistemaRestauranteApi.Models;

namespace SistemaRestauranteApi.Services
{
    public class MesaService : IMesaService
    {
        private readonly AppDbContext _context;

        public MesaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MesaResponseDto>> GetAllAsync()
        {
            return await _context.Set<Mesa>()
                .Select(m => new MesaResponseDto
                {
                    Numero = m.Numero,
                    Capacidad = m.Capacidad,
                    Estado = m.Estado
                })
                .ToListAsync();
        }

        public async Task<MesaResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Set<Mesa>()
                .Where(m => m.Id == id)
                .Select(m => new MesaResponseDto
                {
                    Numero = m.Numero,
                    Capacidad = m.Capacidad,
                    Estado = m.Estado
                })
                .FirstOrDefaultAsync();
        }
    }
}
