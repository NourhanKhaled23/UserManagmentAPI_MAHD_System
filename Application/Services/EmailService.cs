using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Application.Interfaces;

namespace Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpHost = _configuration["Smtp:Host"] ?? throw new InvalidOperationException("SMTP Host configuration is missing");
                var smtpPortString = _configuration["Smtp:Port"] ?? throw new InvalidOperationException("SMTP Port configuration is missing");
                var smtpUsername = _configuration["Smtp:Username"] ?? throw new InvalidOperationException("SMTP Username configuration is missing");
                var smtpPassword = _configuration["Smtp:Password"] ?? throw new InvalidOperationException("SMTP Password configuration is missing");
                var smtpFrom = _configuration["Smtp:From"] ?? throw new InvalidOperationException("SMTP From configuration is missing");

                if (!int.TryParse(smtpPortString, out int smtpPort))
                {
                    throw new InvalidOperationException("Invalid SMTP Port configuration");
                }

                var smtpClient = new SmtpClient(smtpHost)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpFrom),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully to {To}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", to);
                throw new Exception("Failed to send email. Try again later.");
            }
        }
    }
}