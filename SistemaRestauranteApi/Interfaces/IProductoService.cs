
using SistemaRestauranteApi.DTOs;

namespace SistemaRestauranteApi.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoResponseDto>> GetAllAsync();
        Task<ProductoResponseDto?> GetByIdAsync(int id);
    }
}
