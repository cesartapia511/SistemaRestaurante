
using Microsoft.AspNetCore.Mvc;
using SistemaRestauranteApi.DTOs;
using SistemaRestauranteApi.Interfaces;
using SistemaRestauranteApi.Services;

namespace SistemaRestauranteApi.Controllers
{
    [ApiController]
    [Route("api/v1/productos")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<List<ProductoResponseDto>> ObtenerTodos()
        {
            return await _productoService.ObtenerTodosAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ProductoResponseDto?> BuscarProductoPorId(int id)
        {
            return await _productoService.BuscarProductoPorIdAsync(id);
        }

        [HttpPost]
        public async Task<ProductoResponseDto?> CrearProducto(ProductoRequestDto producto)
        {
            return await _productoService.CrearProductoAsync(producto);
        }

        [HttpPatch("{id:int}")]
        public async Task<ProductoResponseDto?> ActualizarProducto(int id, ProductoRequestDto producto)
        {
            return await _productoService.ActualizarProducto(id, producto);
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> EliminarProducto(int id)
        {
            return await _productoService.EliminarAsync(id);
        }

        
    }
}
