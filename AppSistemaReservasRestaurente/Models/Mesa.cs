using System.ComponentModel.DataAnnotations;

namespace AppSistemaReservasRestaurente.Models
{
    public class Mesa
    {
        [Key]
        public int ID_Mesa { get; set; } // PK

        [Required(ErrorMessage = "La capacidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor que 0")]
        public int Capacidad { get; set; }

        [Required(ErrorMessage = "La zona es obligatoria")]
        [StringLength(50, ErrorMessage = "La zona no puede superar los 50 caracteres")]
        public string Zona { get; set; } = "";

        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
