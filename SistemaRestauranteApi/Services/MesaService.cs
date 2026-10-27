
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

        public async Task<List<MesaResponseDto>> ObtenerTodosAsync()
        {
            return await _context.Mesas.Select(m => new MesaResponseDto()
                                        {
                                            Id = m.Id,
                                            Numero = m.Numero,
                                            Capacidad = m.Capacidad,
                                            Estado = m.Estado
                                        })
                                        .ToListAsync();
        }

        public async Task<MesaResponseDto?> BuscarMesaPorIdAsync(int id)
        {
            var existe = await _context.Mesas.FirstOrDefaultAsync(m => m.Id == id);

            if (existe is null)
            {
                return null;
            }

            return new MesaResponseDto()
            {
                Id = existe.Id,
                Numero = existe.Numero,
                Capacidad = existe.Capacidad,
                Estado = existe.Estado
            };
        }

        public async Task<MesaResponseDto?> CrearMesaAsync( MesaRequestDto mesa)
        {
            var numeroExiste = await _context.Mesas
                .AnyAsync(m => m.Numero == mesa.Numero);

            if (numeroExiste)
            {
                return null;
            }

            var nuevaMesa = new Mesa()
            {
                Numero = mesa.Numero,
                Capacidad = mesa.Capacidad,
                Estado = "Disponible"
            };

            _context.Mesas.Add(nuevaMesa);
            await _context.SaveChangesAsync();

            return await BuscarMesaPorIdAsync(nuevaMesa.Id);
        }

        public async Task<MesaResponseDto?> ActualizarMesa(int id, MesaRequestDto mesa)
        {
            var existe = await _context.Mesas.FirstOrDefaultAsync(m => m.Id == id);

            if (existe is null)
            {
                return null;
            }

            var numeroExiste = await _context.Mesas.AnyAsync(m => m.Numero == mesa.Numero && m.Id != id);

            if (numeroExiste)
            {
                return null;
            }

            existe.Numero = mesa.Numero;
            existe.Capacidad = mesa.Capacidad;

            await _context.SaveChangesAsync();

            return await BuscarMesaPorIdAsync(existe.Id);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var existe = await _context.Mesas.FirstOrDefaultAsync(m => m.Id == id);

            if (existe is null)
            {
                return false;
            }

            var tienePedidos = await _context.Pedidos.AnyAsync(p => p.MesaId == id);

            if (tienePedidos)
            {
                return false;
            }

            _context.Mesas.Remove(existe);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
