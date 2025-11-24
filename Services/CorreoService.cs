using Microsoft.Extensions.Options;
using Sistema_Web_de_Nominas.Models.Settings;
using System.Net.Mail;
using System.Net;

namespace Sistema_Web_de_Nominas.Services
{
    public class CorreoService(IOptions<SmtpSettings> smtpOptions) : ICorreoService
    {
            private readonly SmtpSettings _smtp = smtpOptions.Value;
            public void EnvioCorreoReinicioContra(string toEmail, string body)
            {
                if (string.IsNullOrWhiteSpace(toEmail))
                {
                    throw new ArgumentException("Recipient email cannot be null or empty.", nameof(toEmail));
                }
                using var client = new SmtpClient(_smtp.Host, _smtp.Port)
                {
                    Credentials = new NetworkCredential(_smtp.Username, _smtp.Password),
                    EnableSsl = _smtp.EnableSsl
                };
                var mail = new MailMessage
                {
                    From = new MailAddress(_smtp.Username, "Soporte de Autenticacion"),
                    Subject = "Token de Confirmacion/Recuperacion",
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(toEmail);
                client.Send(mail);
            }
    }
}
