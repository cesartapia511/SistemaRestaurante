
namespace SistemaRestauranteApi.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Telefono { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }
            = new List<Pedido>();
    }
}
