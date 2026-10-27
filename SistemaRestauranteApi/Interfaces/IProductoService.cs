
using SistemaRestauranteApi.DTOs;

namespace SistemaRestauranteApi.Interfaces
{
    public interface IProductoService
    {
        Task<List<ProductoResponseDto>> ObtenerTodosAsync();

        Task<ProductoResponseDto?> BuscarProductoPorIdAsync(int id);

        Task<ProductoResponseDto?> CrearProductoAsync(ProductoRequestDto producto);

        Task<ProductoResponseDto?> ActualizarProducto(int id, ProductoRequestDto producto);

        Task<bool> EliminarAsync(int id);
    }
}
