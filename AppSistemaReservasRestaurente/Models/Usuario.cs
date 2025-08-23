using System.ComponentModel.DataAnnotations;

namespace AppSistemaReservasRestaurente.Models
{
    public class Usuario
    {
        [Key]
        public int ID_Usuario { get; set; } // PK
        public string Correo { get; set; } = string.Empty;       // varchar(100)
        public string Contrasena { get; set; } = string.Empty;   // varchar(100) encriptada
        public string Rol { get; set; } = "Cliente";             // varchar(20) (ej: "Administrador", "Cliente")

        // Relación con Clientes (un usuario puede estar ligado a un cliente)
        public Cliente? Cliente { get; set; }
    }
}
