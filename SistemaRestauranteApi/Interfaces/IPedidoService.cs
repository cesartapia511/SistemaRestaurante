using SistemaRestauranteApi.DTOs;

namespace SistemaRestauranteApi.Interfaces;

public interface IPedidoService
{
    Task<List<PedidoResponseDto>> ObtenerTodosAsync();

    Task<PedidoResponseDto?> BuscarPedidoPorIdAsync(int id);

    Task<PedidoResponseDto?> CrearPedidoAsync(PedidoRequestDto pedido);

    Task<PedidoResponseDto?> ActualizarPedidoAsync(int id, PedidoRequestDto pedido);

    Task<bool> EliminarPedidoAsync(int id);

    Task<PedidoResponseDto?> CambiarEstadoAsync(int id, PedidoEstadoRequestDto pedidoEstado);

    Task<List<PedidoResponseDto>> BuscarPorMesaAsync(int mesaId);

    Task<List<PedidoResponseDto>> BuscarPorFechaAsync(DateTime fecha);

    Task<List<PedidoResponseDto>> BuscarPorMetodoPagoAsync(string metodoPago);
    
}