using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Polly;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string htmlMessage);
}

public class EmailSender : IEmailSender
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _fromAddress;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
    {
        var emailSettings = configuration.GetSection("EmailSettings");

        _smtpHost = emailSettings["SmtpHost"];
        _smtpPort = int.Parse(emailSettings["SmtpPort"]);
        _fromAddress = emailSettings["SmtpFromAddress"];
        _smtpUsername = emailSettings["SmtpUsername"];
        _smtpPassword = emailSettings["SmtpPassword"];
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_fromAddress));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart("html") { Text = htmlMessage };

        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync(_smtpHost, _smtpPort, true);  // 비동기 연결
        await smtpClient.AuthenticateAsync(_smtpUsername, _smtpPassword);  // 비동기 인증
        await smtpClient.SendAsync(email);  // 비동기 이메일 전송
        await smtpClient.DisconnectAsync(true);  // 비동기 연결 해제
    }
}