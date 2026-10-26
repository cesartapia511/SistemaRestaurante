namespace SistemaRestauranteApi.Models;

public class Pedido
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public decimal Total { get; set; }
    public string? MetodoPago { get; set; }

    public int MesaId { get; set; }
    public Mesa? Mesa { get; set; } = null!;

    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; } = null!;

    public ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
}