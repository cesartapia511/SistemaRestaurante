
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
            _context = context;
        }

        public async Task<IEnumerable<ClienteResponseDto>> GetAllAsync()
        {
            return await _context.Set<Cliente>()
                .Select(c => new ClienteResponseDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Telefono = c.Telefono
                })
                .ToListAsync();
        }

        public async Task<ClienteResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Set<Cliente>()
                .Where(c => c.Id == id)
                .Select(c => new ClienteResponseDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Telefono = c.Telefono
                })
                .FirstOrDefaultAsync();
        }
    }
}
