using System.ComponentModel.DataAnnotations;

namespace AppSistemaReservasRestaurente.Models
{
    public class ReservaHorarioVM
    {
        
        public string Hora { get; set; }
        public int Mesa { get; set; }
        public string NombreCliente { get; set; }
        public int CantidadPersonas { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Codigo { get; set; }
        public string Estado { get; set; }
    }

}
