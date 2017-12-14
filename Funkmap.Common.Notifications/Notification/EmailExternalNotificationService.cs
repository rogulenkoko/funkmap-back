using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Funkmap.Common.Logger;
using Funkmap.Common.Notifications.Notification.Abstract;
using Funkmap.Common.Settings;

namespace Funkmap.Common.Notifications.Notification
{
    public class EmailExternalNotificationService : IExternalNotificationService
    {
        private readonly string _appEmail;
        private readonly string _appEmailPassword;
        private readonly IFunkmapLogger<EmailExternalNotificationService> _logger;


        public EmailExternalNotificationService(ISettingsService settingsService, IFunkmapLogger<EmailExternalNotificationService> logger)
        {
            var settings = settingsService.GetSettings();
            if (settings == null) throw new ArgumentNullException(nameof(settings), "Необходим сервис настроек и настройки");
            _appEmail = settings.Email;
            _appEmailPassword = settings.EmailPassword;
            _logger = logger;
        }

        public async Task<bool> SendNotification(Notification notification)
        {
            await Task.Yield();
            try
            {
                _logger.Info($"Отправка email по адресу {notification.Receiver}");

                string root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                var body = File.ReadAllText(new Uri(Path.Combine(root, "Templates/base-template.html")).LocalPath);

                var mailDefenition = new MailDefinition
                {
                    From = _appEmail,
                    Subject = notification.Subject,
                    IsBodyHtml = true
                };

                var replacements = new Dictionary<string, string>()
                {
                    {"title", notification.Title },
                    {"main", notification.MainContent },
                    {"footer", notification.Footer }
                };

                var message = mailDefenition.CreateMailMessage(notification.Receiver, replacements, body, new System.Web.UI.Control());

                var smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.mail.ru";
                smtpClient.Port = 25;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(_appEmail, _appEmailPassword);
                smtpClient.Send(message);
                message.Dispose();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Произошла ошибка отправки email");
                return false;
            }

        }
    }
}
