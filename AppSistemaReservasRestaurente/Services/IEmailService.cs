using AppSistemaReservasRestaurente.Models;

namespace AppSistemaReservasRestaurente.Services
{
    public interface IEmailService
    {
        Task<bool> EnviarCorreoReservaAsync(string destinatario, string nombreCliente, Reserva reserva);
    }
}