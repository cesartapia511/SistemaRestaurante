
using Microsoft.AspNetCore.Mvc;
using SistemaRestauranteApi.DTOs;
using SistemaRestauranteApi.Interfaces;
using SistemaRestauranteApi.Services;

namespace SistemaRestauranteApi.Controllers
{
    [ApiController]
    [Route("api/v1/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<List<ClienteResponseDto>> ObtenerTodos()
        {
            return await _clienteService.ObtenerTodosAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ClienteResponseDto?> BuscarClientePorId(int id)
        {
            return await _clienteService.BuscarClientePorIdAsync(id);
        }

        [HttpPost]
        public async Task<ClienteResponseDto?> CrearCliente(ClienteRequestDto cliente)
        {
            return await _clienteService.CrearClienteAsync(cliente);
        }

        [HttpPatch("{id:int}")]
        public async Task<ClienteResponseDto?> ActualizarCliente(int id, ClienteRequestDto cliente)
        {
            return await _clienteService.ActualizarCliente(id, cliente);
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> EliminarCliente(int id)
        {
            return await _clienteService.EliminarAsync(id);
        }
    }
}
