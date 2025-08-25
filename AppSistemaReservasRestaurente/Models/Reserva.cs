using System.ComponentModel.DataAnnotations;

namespace AppSistemaReservasRestaurente.Models
{
    public class Reserva
    {
        [Key]
        public int ID_Reserva { get; set; } // PK

        // Relaciones (FKs)
        public int ID_Cliente { get; set; }
        public Cliente? Cliente { get; set; } = default!;

        public int ID_Mesa { get; set; }
        public Mesa? Mesa { get; set; } = default!;

        public int ID_Horario { get; set; }
        public Horario? Horario { get; set; } = default!;

        // Otros atributos
        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La cantidad de personas es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de personas debe ser mayor que 0")]
        public int Cantidad_Personas { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(20, ErrorMessage = "El estado no puede superar los 20 caracteres")]
        public string Estado { get; set; } = "Confirmado";
    }
}
