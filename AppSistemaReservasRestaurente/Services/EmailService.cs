using System.Net;
using System.Net.Mail;
using AppSistemaReservasRestaurente.Models;
using AppSistemaReservasRestaurente.Data;
using Microsoft.EntityFrameworkCore;

namespace AppSistemaReservasRestaurente.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly BDContexto _context;

        public EmailService(IConfiguration configuration, BDContexto context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<bool> EnviarCorreoReservaAsync(string destinatario, string nombreCliente, Reserva reserva)
        {
            try
            {
                // Obtener datos relacionados de la base de datos
                var reservaCompleta = await _context.Reservas
                    .Include(r => r.Cliente)
                    .Include(r => r.Mesa)
                    .Include(r => r.Horario)
                    .FirstOrDefaultAsync(r => r.ID_Reserva == reserva.ID_Reserva);

                if (reservaCompleta == null)
                    return false;

                // Configurar cliente SMTP
                var smtpHost = _configuration["Smtp:Host"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["Smtp:Port"] ?? "587");
                var smtpUsername = _configuration["Smtp:Username"];
                var smtpPassword = _configuration["Smtp:Password"];
                var fromEmail = _configuration["Smtp:FromEmail"] ?? smtpUsername;
                var fromName = _configuration["Restaurante:Nombre"] ?? "Restaurante";

                using (var client = new SmtpClient(smtpHost, smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                    // Crear el mensaje de correo
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail, fromName),
                        Subject = $"Confirmación de Reserva - {fromName}",
                        IsBodyHtml = true,
                        Body = CrearCuerpoHtmlReserva(reservaCompleta, nombreCliente, fromName)
                    };

                    mailMessage.To.Add(new MailAddress(destinatario, nombreCliente));

                    // Enviar el correo
                    await client.SendMailAsync(mailMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log del error (puedes usar ILogger aquí)
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
                return false;
            }
        }

        private string CrearCuerpoHtmlReserva(Reserva reserva, string nombreCliente, string nombreRestaurante)
        {
            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .details {{ background-color: white; padding: 15px; margin: 10px 0; border-radius: 5px; }}
                        .footer {{ text-align: center; padding: 20px; color: #666; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>¡Reserva Confirmada!</h1>
                        </div>
                        <div class='content'>
                            <p>Estimado/a {nombreCliente},</p>
                            <p>Su reserva ha sido confirmada exitosamente. A continuación los detalles:</p>
                            
                            <div class='details'>
                                <h3>Detalles de la Reserva</h3>
                                <p><strong>Número de Reserva:</strong> {reserva.ID_Reserva}</p>
                                <p><strong>Fecha:</strong> {reserva.Fecha:dd/MM/yyyy}</p>
                                <p><strong>Horario:</strong> {reserva.Horario?.Hora_Inicio:HH:mm} - {reserva.Horario?.Hora_Final:HH:mm}</p>
                                <p><strong>Mesa:</strong> {reserva.Mesa?.ID_Mesa} (Zona: {reserva.Mesa?.Zona})</p>
                                <p><strong>Capacidad:</strong> {reserva.Mesa?.Capacidad} personas</p>
                                <p><strong>Cantidad de Personas:</strong> {reserva.Cantidad_Personas}</p>
                                <p><strong>Estado:</strong> {reserva.Estado}</p>
                            </div>
                            
                            <p>Por favor, llegue 10 minutos antes de su horario reservado.</p>
                            <p>Si necesita modificar o cancelar su reserva, contáctenos con anticipación.</p>
                        </div>
                        <div class='footer'>
                            <p>Gracias por elegir {nombreRestaurante}</p>
                            <p>¡Esperamos verle pronto!</p>
                        </div>
                    </div>
                </body>
                </html>";
        }
    }
}