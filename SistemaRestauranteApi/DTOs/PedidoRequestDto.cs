using System.ComponentModel.DataAnnotations;

namespace SistemaRestauranteApi.DTOs;
public class PedidoRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una mesa.")]
    public int MesaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente.")]
    public int ClienteId { get; set; }

    [StringLength(30, ErrorMessage = "El método de pago admite hasta 30 caracteres.")]
    public string? MetodoPago { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "El pedido debe tener al menos un producto.")]
    public List<DetallePedidoRequestDto> Detalles { get; set; } = new();
}

public class DetallePedidoRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto.")]
    public int ProductoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
    public int Cantidad { get; set; }
}