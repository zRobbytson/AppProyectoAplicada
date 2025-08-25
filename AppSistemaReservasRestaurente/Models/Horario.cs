using System.ComponentModel.DataAnnotations;

namespace AppSistemaReservasRestaurente.Models
{
    public class Horario
    {
        [Key]
        public int ID_Horario { get; set; } // PK

        [Required(ErrorMessage = "La hora de inicio es obligatoria")]
        public TimeOnly Hora_Inicio { get; set; }

        [Required(ErrorMessage = "La hora final es obligatoria")]
        public TimeOnly Hora_Final { get; set; }

        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
