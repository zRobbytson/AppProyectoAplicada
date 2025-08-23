using System.ComponentModel.DataAnnotations;

namespace AppSistemaReservasRestaurente.Models
{
    public class Reserva
    {
        [Key]
        public int ID_Reserva { get; set; } // PK

        // Relaciones (FKs)
        public int ID_Cliente { get; set; }
        public Cliente Cliente { get; set; } = default!;

        public int ID_Mesa { get; set; }
        public Mesa Mesa { get; set; } = default!;

        public int ID_Horario { get; set; }
        public Horario Horario { get; set; } = default!;

        // Otros atributos
        public DateTime Fecha { get; set; }           // datetime
        public int Cantidad_Personas { get; set; }     // int
        public string Estado { get; set; } = "Pendiente"; // varchar(20) (Pendiente, Confirmada, Cancelada)
    }
}
