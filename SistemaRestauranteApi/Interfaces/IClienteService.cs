
using SistemaRestauranteApi.DTOs;

namespace SistemaRestauranteApi.Interfaces
{
    public interface IClienteService
    {
        Task<List<ClienteResponseDto>> ObtenerTodosAsync();

        Task<ClienteResponseDto?> BuscarClientePorIdAsync(int id);

        Task<ClienteResponseDto?> CrearClienteAsync(ClienteRequestDto cliente);

        Task<ClienteResponseDto?> ActualizarCliente(int id, ClienteRequestDto cliente);

        Task<bool> EliminarAsync(int id);
    }
}
