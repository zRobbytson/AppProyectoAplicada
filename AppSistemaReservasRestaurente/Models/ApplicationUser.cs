using Microsoft.AspNetCore.Identity;

namespace AppSistemaReservasRestaurente.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        // Relación 1:1 con Cliente
        public Cliente? Cliente { get; set; }
    }
}
