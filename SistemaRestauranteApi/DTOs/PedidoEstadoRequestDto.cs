using System.ComponentModel.DataAnnotations;

namespace SistemaRestauranteApi.DTOs;

public class PedidoEstadoRequestDto
{
    [Required(ErrorMessage = "Debe indicar el estado del pedido.")]
    [StringLength(20, ErrorMessage = "El estado admite hasta 20 caracteres.")]
    public string Estado { get; set; } = string.Empty;
}