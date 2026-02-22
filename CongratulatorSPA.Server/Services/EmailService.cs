using CongratulatorSPA.Server.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace CongratulatorSPA.Server.Services
{
    public class EmailService(IConfiguration config) : IEmailService
    {
        public async Task SendBirthdayAsync(string email, string name, CancellationToken cancellationToken)
        {
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse(config["Email:From"]!));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = $"С днем рождения, {name}!";
            message.Body = new TextPart("plain")
            {
                Text = $"Привет {name}! Поздравляем тебя с днем рождения!"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(config["Email:Host"], 587, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(config["Email:User"], config["Email:Password"], cancellationToken);
            await smtp.SendAsync(message, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}
