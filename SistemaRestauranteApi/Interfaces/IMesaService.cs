
using SistemaRestauranteApi.DTOs;

namespace SistemaRestauranteApi.Interfaces
{
    public interface IMesaService
    {
       Task<List<MesaResponseDto>> ObtenerTodosAsync();

        Task<MesaResponseDto?> BuscarMesaPorIdAsync(int id);

        Task<MesaResponseDto?> CrearMesaAsync(MesaRequestDto mesa);

        Task<MesaResponseDto?> ActualizarMesa(int id, MesaRequestDto mesa);

        Task<bool> EliminarAsync(int id);
    }
}
