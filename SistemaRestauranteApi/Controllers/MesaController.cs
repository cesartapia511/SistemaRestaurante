
using Microsoft.AspNetCore.Mvc;
using SistemaRestauranteApi.DTOs;
using SistemaRestauranteApi.Interfaces;
using SistemaRestauranteApi.Services;

namespace SistemaRestauranteApi.Controllers
{
    [ApiController]
    [Route("api/v1/mesas")]
    public class MesaController : ControllerBase
    {
        private readonly MesaService _mesaService;

        public MesaController(MesaService mesaService)
        {
            _mesaService = mesaService;
        }

        [HttpGet]
        public async Task<List<MesaResponseDto>> ObtenerTodos()
        {
            return await _mesaService.ObtenerTodosAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<MesaResponseDto?> BuscarMesaPorId(int id)
        {
            return await _mesaService.BuscarMesaPorIdAsync(id);
        }

        [HttpPost]
        public async Task<MesaResponseDto?> CrearMesa(MesaRequestDto mesa)
        {
            return await _mesaService.CrearMesaAsync(mesa);
        }

        [HttpPatch("{id:int}")]
        public async Task<MesaResponseDto?> ActualizarMesa(
            int id, MesaRequestDto mesa)
        {
            return await _mesaService.ActualizarMesa(id, mesa);
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> EliminarMesa(int id)
        {
            return await _mesaService.EliminarAsync(id);
        }
    }
}
