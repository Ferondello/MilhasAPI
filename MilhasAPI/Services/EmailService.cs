using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MilhasAPI.Configuration;
using MilhasAPI.Services.Interfaces;
using MimeKit;

namespace MilhasAPI.Services;

public class EmailService : IEmailService
{
    private readonly EmailOptions _options;

    public EmailService(IOptions<EmailOptions> options)
    {
        _options = options.Value;
    }

    public async Task SendAsync(string toEmail, string toName, string subject, string htmlBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.FromName, _options.FromAddress));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            _options.SmtpHost,
            _options.SmtpPort,
            _options.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(_options.Username, _options.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
