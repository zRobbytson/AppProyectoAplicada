namespace AppSistemaReservasRestaurente.Models
{
    public class Horario
    {
        public int HorarioId { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }

        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
