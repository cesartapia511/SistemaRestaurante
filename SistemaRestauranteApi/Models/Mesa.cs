
namespace SistemaRestauranteApi.Models
{
    public class Mesa
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int Capacidad { get; set; }
        public string Estado { get; set; } = "Disponible";

        public ICollection<Pedido> Pedidos { get; set; }
            = new List<Pedido>();
    }
}
