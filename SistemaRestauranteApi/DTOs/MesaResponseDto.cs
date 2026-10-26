
namespace SistemaRestauranteApi.DTOs
{
    public class MesaResponseDto
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int Capacidad { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
