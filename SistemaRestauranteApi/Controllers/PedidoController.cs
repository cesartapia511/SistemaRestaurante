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
    public async Task<PedidoResponseDto?> ActualizarPedidoAsync(int id, PedidoRequestDto pedido)
    {
        return await _pedidoService.ActualizarPedidoAsync(id, pedido);
    }

    [HttpGet("mesa/{mesaId:int}")]
    public async Task<List<PedidoResponseDto>> BuscarPorMesaAsync(int mesaId)
    {
        return await _pedidoService.BuscarPorMesaAsync(mesaId);
    }

    [HttpGet("por-fecha")]
    public async Task<List<PedidoResponseDto>> BuscarPorFechaAsync(DateTime fecha)
    {
        return await _pedidoService.BuscarPorFechaAsync(fecha);
    }

    [HttpGet("por-metodo-pago")]
    public async Task<List<PedidoResponseDto>> BuscarPorMetodoPagoAsync(string metodoPago)
    {
        return await _pedidoService.BuscarPorMetodoPagoAsync(metodoPago);
    }

    [HttpPatch("{id:int}/estado")]
    public async Task<PedidoResponseDto?> CambiarEstadoAsync(int id, PedidoEstadoRequestDto pedidoEstado)
    {
        return await _pedidoService.CambiarEstadoAsync(id, pedidoEstado);
    }

}
