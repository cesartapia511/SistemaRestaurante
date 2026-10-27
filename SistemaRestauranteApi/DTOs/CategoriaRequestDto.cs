using System.ComponentModel.DataAnnotations;

public class CategoriaRequestDto
{
    [Required]
    [StringLength(50, ErrorMessage = "La propiedad no tiene la longitud correcta")]
    public string Nombre {get; set; } = string.Empty;

    public string? Descripcion { get; set; }

}