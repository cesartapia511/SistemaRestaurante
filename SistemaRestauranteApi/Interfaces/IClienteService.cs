
using SistemaRestauranteApi.DTOs;

namespace SistemaRestauranteApi.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteResponseDto>> GetAllAsync();
        Task<ClienteResponseDto?> GetByIdAsync(int id);
    }
}
