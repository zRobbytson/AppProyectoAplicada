using System.ComponentModel.DataAnnotations;

namespace AppSistemaReservasRestaurente.Models
{
    public class Horario
    {
        [Key]
        public int ID_Horario { get; set; } // PK
        public TimeOnly Hora_Inicio { get; set; }
        public TimeOnly Hora_Final { get; set; }

        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
