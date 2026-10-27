
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
            this._context = context;
        }

        public async Task<CategoriaResponseDto> ActualizarCategoria(int id, CategoriaRequestDto categoria)
        {
            var existe = await _context.Categorias.FirstAsync(p => p.Id == id);

            if (existe is null)
            {
                return null;
            }

            existe.Nombre = categoria.Nombre;
            existe.Descripcion = categoria.Descripcion;
            

            await _context.SaveChangesAsync();

            return await this.BuscarCategoriaPorIdAsync(existe.Id);
        }

        public async Task<CategoriaResponseDto> BuscarCategoriaPorIdAsync(int id)
        {
            var existe = await _context.Categorias.FirstAsync(p => p.Id == id);

            if (existe is null)
            {
                return null;
            }
            return new CategoriaResponseDto()
            {
                Id = existe.Id,
                Nombre = existe.Nombre,
                Descripcion = existe.Descripcion
            
            };
        }

        public async Task<CategoriaResponseDto> CrearCategoriaAsync(CategoriaRequestDto categoriaRequest)
        {
            var cat = new Categoria()
            {
                Nombre = categoriaRequest.Nombre,
                Descripcion = categoriaRequest.Descripcion
            };
            _context.Categorias.Add(cat);
            await _context.SaveChangesAsync();
            return await this.BuscarCategoriaPorIdAsync(cat.Id);
        }

        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            var existe = await _context.Categorias.FindAsync(id);

            if(existe is null)
            {
                return false;
            }

            _context.Categorias.Remove(existe);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CategoriaResponseDto>> ObtenerTodosAsync()
        {
            return await _context.Categorias.Select(c => new CategoriaResponseDto()
                                            {
                                                Id = c.Id,
                                                Nombre = c.Nombre,
                                                Descripcion = c.Descripcion
                                            }).ToListAsync();
        }
    }

}
