using System.ComponentModel.DataAnnotations;

public class ProductoRequestDto
{
    [Required]
    [StringLength(50, ErrorMessage = "La propiedad no tiene la longitud correcta")]
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }
    
    [Url(ErrorMessage = "Debe ingresar una URL válida.")]
    public string? ImagenUrl { get; set; }

    [Required]
    [Range(1, 1000, ErrorMessage = "El precio no puede ser menor a cero o negativo")]
    public decimal Precio { get; set; }
    public int CategoriaId { get; set; }
}