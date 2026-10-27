using System.ComponentModel.DataAnnotations;

public class MesaRequestDto
{
    [Required]
    [Range(1, 200, ErrorMessage = "El numero debe estar entre 1 y 200.")]
    public int Numero { get; set; }

    [Required]
    [Range(2, 12, ErrorMessage = "La capacidad debe estar entre 2 y 12")]
    public int Capacidad { get; set; }
}