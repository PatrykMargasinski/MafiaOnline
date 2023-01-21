using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Utils
{
    public interface IMailSender
    {
        void SendEmail(string subject, string content, string to);
    }

    public class MailSender : IMailSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MailSender> _logger;

        public MailSender(IConfiguration config, ILogger<MailSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public void SendEmail(string subject, string content, string to)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials =
                    new System.Net.NetworkCredential(_config.GetValue<string>("MailSender:Mail"), _config.GetValue<string>("MailSender:Password"));
                client.Port = 587;
                client.Host = _config.GetValue<string>("MailSender:Host");
                var mail = new MailMessage();
                string addr = to;
                mail.From = new MailAddress(_config.GetValue<string>("MailSender:Mail"), _config.GetValue<string>("MailSender:SenderName"));
                mail.To.Add(addr);
                mail.Subject = subject;
                mail.Body = content;
                mail.IsBodyHtml = true;
                mail.DeliveryNotificationOptions =
                    DeliveryNotificationOptions.OnFailure;
                client.Send(mail);
                _logger.LogDebug("Mail sent to: " + to);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.GetBaseException().ToString());
            }
        }
        
    }
}
