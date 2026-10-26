
using Microsoft.AspNetCore.Mvc;
using SistemaRestauranteApi.Interfaces;

namespace SistemaRestauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MesaController : ControllerBase
    {
        private readonly IMesaService _mesaService;

        public MesaController(IMesaService mesaService)
        {
            _mesaService = mesaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mesas = await _mesaService.GetAllAsync();
            return Ok(mesas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var mesa = await _mesaService.GetByIdAsync(id);

            if (mesa == null)
            {
                return NotFound();
            }

            return Ok(mesa);
        }
    }
}
