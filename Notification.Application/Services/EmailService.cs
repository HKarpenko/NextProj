using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Notification.Domain.ConfigurationModels;
using Microsoft.Extensions.Logging;
using Serilog;
using Notification.Application.Interfaces;

namespace Notification.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        private const string subject = "NextProj Event Notification";

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string message)
        {
            try
            {
                var body = CreateEmailMessage(message);
                using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                    client.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage);
                }

                Log.Information("Email sent successfully to {Recipient} with subject: {Subject}", toEmail, subject);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error sending email to {Recipient}: {Message}", toEmail, ex.Message);
            }
        }

        private string CreateEmailMessage(string message)
        {
            string sendMessage = "This is Event Notification from NextProj.";
            sendMessage += message == null ? "" : $"<br>Message:<br>{message}";
            return sendMessage;
        }
    }

}
