
namespace SistemaRestauranteApi.DTOs
{
    public class ClienteResponseDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Telefono { get; set; }
    }
}
