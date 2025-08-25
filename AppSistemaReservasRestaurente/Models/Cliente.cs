using System.ComponentModel.DataAnnotations;

namespace AppSistemaReservasRestaurente.Models
{
    public class Cliente
    {
        [Key]
        public int ID_Cliente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre_Cliente { get; set; } = string.Empty;

        [MaxLength(9)]
        [StringLength(9, ErrorMessage = "El teléfono no puede superar los 9 caracteres")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener exactamente 8 caracteres")]
        [MaxLength(8)]
        public string DNI { get; set; } = string.Empty;

        // Relación con Usuario
        [Required]
        public int Id { get; set; }
        public ApplicationUser? Usuario { get; set; }

        // Relación con Reservas
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
