using MailKit.Net.Smtp;
using MimeKit;
using static Seguridad.Core.Entities.EmailConfiguration;

namespace Seguridad.Core.Services
{
    public class EnviarCorreos
	{
        private readonly IConfiguration _configuration;
        public EnviarCorreos(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> SendMailHtml(MailData mailData)
        {
             try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_configuration["NetMail:Usuario"], _configuration["NetMail:Cuenta"]!);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    //string filePath = Directory.GetCurrentDirectory() + "/Resources/templates/RecoveryPass.html";

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = mailData.HtmlBody;
                    //emailBodyBuilder.TextBody = mailData.EmailBody;
                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one

                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.Connect(_configuration["NetMail:Host"]!, int.Parse(_configuration["NetMail:Puerto"]!), MailKit.Security.SecureSocketOptions.SslOnConnect);
                        mailClient.Authenticate(_configuration["NetMail:Cuenta"]!, _configuration["NetMail:Password"]!);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true); 
                    }
                }

                return Task.FromResult(true);

             }
                catch (Exception ex)
             {
                 throw new Exception(ex.Message);
               
             }

        }
	}
}

