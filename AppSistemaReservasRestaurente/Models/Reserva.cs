namespace AppSistemaReservasRestaurente.Models
{
    public class Reserva
    {
        public int ReservaId { get; set; }

        public int ClienteId { get; set; }
        public int MesaId { get; set; }
        public int HorarioId { get; set; }


        public DateTime Fecha { get; set; }
        public int NumPersonas { get; set; }
        public string Estado { get; set; } = "Confirmada";

        public Cliente? Cliente { get; set; }
        public Mesa? Mesa { get; set; }
        public Horario? Horario { get; set; } 
    }
}
