using System.ComponentModel.DataAnnotations;

public class MesaRequestDto
{
    [Required]
    [Range(1, 200, ErrorMessage = "El numero tiene un valor negativo o cero")]
    public int Numero { get; set; }

    [Required]
    [Range(1, 12, ErrorMessage = "La capacidad tiene un valor negativo o cero")]
    public int Capacidad { get; set; }
}