using System.ComponentModel.DataAnnotations;

namespace AppSistemaReservasRestaurente.Models
{
    public class Mesa
    {
        [Key]
        public int ID_Mesa { get; set; } // PK
        public int Capacidad { get; set; }       // int
        public string Zona { get; set; } = "";   // varchar(50)

        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
