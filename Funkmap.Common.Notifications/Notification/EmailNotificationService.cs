using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Logger;
using Funkmap.Common.Notifications.Notification.Abstract;
using Funkmap.Common.Settings;

namespace Funkmap.Common.Notifications.Notification
{
    public class EmailNotificationService : INotificationService
    {
        private readonly string _appEmail;
        private readonly string _appEmailPassword;
        private readonly IFunkmapLogger<EmailNotificationService> _logger;


        public EmailNotificationService(ISettingsService settingsService, IFunkmapLogger<EmailNotificationService> logger)
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
                _logger.Info($"ОТправка email по адресу {notification.Receiver}");

                var message = new MailMessage();
                message.From = new MailAddress(_appEmail);
                message.To.Add(new MailAddress(notification.Receiver));
                message.Body = notification.Body;
                message.Subject = notification.Subject;

                var smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";
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
