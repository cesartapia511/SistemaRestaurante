using System.ComponentModel.DataAnnotations;

public class ClienteRequestDto
{
    [Required]
    [StringLength(50, ErrorMessage = "La propiedad no tiene la longitud correcta")]
    public string Nombre { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50, ErrorMessage = "La propiedad no tiene la longitud correcta")]
    public string Apellido { get; set; } = string.Empty;
    
    public string? Telefono { get; set; }

}