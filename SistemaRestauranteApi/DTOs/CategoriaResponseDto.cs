
namespace SistemaRestauranteApi.DTOs
{
    public class CategoriaResponseDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}
