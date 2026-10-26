
using Microsoft.AspNetCore.Mvc;
using SistemaRestauranteApi.Interfaces;

namespace SistemaRestauranteApi.Controllers
{
    [ApiController]
    [Route("api/v1/categorias")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias = await _categoriaService.GetAllAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var categoria = await _categoriaService.GetByIdAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }
    }
}
