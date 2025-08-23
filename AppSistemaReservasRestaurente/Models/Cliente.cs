using System.ComponentModel.DataAnnotations;

namespace AppSistemaReservasRestaurente.Models
{
    public class Cliente
    {
        [Key]
        public int ID_Cliente { get; set; }
        public string Nombre_Cliente { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string DNI { get; set; } = string.Empty;

        // Relación con Usuario
        public int Id { get; set; }
        public ApplicationUser? Usuario { get; set; }

        // Relación con Reservas
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
