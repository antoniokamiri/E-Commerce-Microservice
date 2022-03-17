using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Model;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Services
{
    public class MailService : IEmailService
    {
        public EmailSettings _emailSetting { get; }
        public ILogger<EmailSettings> _logger { get; }

        // we are using IOption to get Email Setting updated automatically when the source changes 
        public MailService(IOptions<EmailSettings> emailSetting, ILogger<EmailSettings> logger)
        {
            _emailSetting = emailSetting.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(Email email)
        {
            var client = new SendGridClient(_emailSetting.ApiKey);

            var from = new EmailAddress
            {
                Email = _emailSetting.FromAddress,
                Name = _emailSetting.FromName
            };
            var subject = email.Subject;
            var to = new EmailAddress(email.To, email.ToName);
            var emailBody = email.Body;

            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
            var response = await client.SendEmailAsync(sendGridMessage);

            _logger.LogInformation("Email sent.");

            if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
                return true;

            _logger.LogError("Email sending failed.");
            return false;
        }
    }
}
