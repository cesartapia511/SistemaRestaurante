
using Microsoft.AspNetCore.Mvc;
using SistemaRestauranteApi.DTOs;
using SistemaRestauranteApi.Interfaces;
using SistemaRestauranteApi.Services;

namespace SistemaRestauranteApi.Controllers
{
    [ApiController]
    [Route("api/v1/categorias")]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<List<CategoriaResponseDto>> ObtenerTodos()
        {
            return await _categoriaService.ObtenerTodosAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<CategoriaResponseDto> BuscarCategoriaId(int id)
        {
            return await _categoriaService.BuscarCategoriaPorIdAsync(id);
        }


        [HttpDelete("{id:int}")]
        public async Task<bool> EliminarCategoria(int id)
        {
            return await _categoriaService.EliminarCategoriaAsync(id);
        }

        [HttpPost]
        public async Task<CategoriaResponseDto> CrearCategoria(CategoriaRequestDto categoria)
        {
            return await _categoriaService.CrearCategoriaAsync(categoria);
        }

        [HttpPatch("{id:int}")] //Por URL
        public async Task<CategoriaResponseDto> ActualizarCategoria(int id, CategoriaRequestDto categoria)
        {
            return await _categoriaService.ActualizarCategoria(id, categoria);
        }
    }
}
