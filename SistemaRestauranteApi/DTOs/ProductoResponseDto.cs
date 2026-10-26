
namespace SistemaRestauranteApi.DTOs
{
    public class ProductoResponseDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }
        public int CategoriaId { get; set; }
    }
}
