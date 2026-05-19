using MailKit.Net.Smtp;
using MailKit.Security;
using MilhasAPI.Services.Interfaces;
using MimeKit;

namespace MilhasAPI.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendAsync(string toEmail, string toName, string subject, string htmlBody)
    {
        var cfg = _config.GetSection("Email");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            cfg["FromName"] ?? "MilhasGerais",
            cfg["FromAddress"]!
        ));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();

        var port   = cfg.GetValue<int>("SmtpPort", 587);
        var useSsl = cfg.GetValue<bool>("UseSsl", false);

        await client.ConnectAsync(
            cfg["SmtpHost"]!,
            port,
            useSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(cfg["Username"]!, cfg["Password"]!);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
