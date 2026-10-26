
using SistemaRestauranteApi.DTOs;

namespace SistemaRestauranteApi.Interfaces
{
    public interface IMesaService
    {
        Task<IEnumerable<MesaResponseDto>> GetAllAsync();
        Task<MesaResponseDto?> GetByIdAsync(int id);
    }
}
