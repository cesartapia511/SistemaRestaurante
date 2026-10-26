using Microsoft.AspNetCore.Mvc;
using SistemaRestauranteApi.DTOs;

[ApiController]

[Route("api/v1/pedidos")]

public class PedidoController : ControllerBase
{
    private PedidoService _pedidoService;

    public PedidoController(PedidoService pedidoService)
    {
        this._pedidoService = pedidoService;
    }

    [HttpGet]
    public async Task<List<PedidoResponseDto>> ObtenerTodos()
    {
        return await _pedidoService.ObtenerTodosAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<PedidoResponseDto?> BuscarPedidoPorIdAsync(int id)
    {
        return await _pedidoService.BuscarPedidoPorIdAsync(id);
    }


    [HttpDelete("{id:int}")]
    public async Task<bool> EliminarPedidoAsync(int id)
    {
        return await _pedidoService.EliminarPedidoAsync(id);
    }

    [HttpPost]
    public async Task<PedidoResponseDto?> CrearPedidoAsync(PedidoRequestDto pedido)
    {
        return await _pedidoService.CrearPedidoAsync(pedido);
    }

    [HttpPatch("{id:int}")] //Por URL
    public async Task<PedidoResponseDto?> ActualizarPedidoAsync(int id, PedidoRequestDto categoria)
    {
        return await _pedidoService.ActualizarPedidoAsync(id, categoria);
    }

}
