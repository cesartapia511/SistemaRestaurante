namespace SistemaRestauranteApi.DTOs;

public class PedidoResponseDto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string? MetodoPago { get; set; }

    public int MesaId { get; set; }
    public int NumeroMesa { get; set; }

    public int ClienteId { get; set; }
    public string NombreCliente { get; set; } = string.Empty;

    public List<DetallePedidoResponseDto> Detalles { get; set; } = new();
}

public class DetallePedidoResponseDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}