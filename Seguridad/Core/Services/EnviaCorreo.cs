
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Seguridad.Core.Services
{
	public class EnviaCorreo : IEmailSender
	{
        private readonly IConfiguration _configuration;

        public EnviaCorreo(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task SendEmailAsync(string para, string asunto, string cuerpo)
        {
            string host = _configuration["NetMail:Host"]!;
            int puerto = int.Parse(_configuration["NetMail:Puerto"]!);
            string usuario = _configuration["NetMail:Usuario"]!;
            string password = _configuration["NetMail:Password"]!;

            SmtpClient client = new(host, puerto);
            client.Credentials = new NetworkCredential(usuario, password);

            MailMessage mensaje = new MailMessage();
            mensaje.From = new MailAddress(_configuration["NetMail:Usuario"]!);
            mensaje.To.Add(para);
            mensaje.Subject = asunto;
            mensaje.Body = cuerpo;

            try
            {
                await client.SendMailAsync(mensaje);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error al enviar");
            }
            
        }

    }
}

