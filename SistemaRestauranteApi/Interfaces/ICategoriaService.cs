
using SistemaRestauranteApi.DTOs;

namespace SistemaRestauranteApi.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaResponseDto>> GetAllAsync();
        Task<CategoriaResponseDto?> GetByIdAsync(int id);
    }
}
