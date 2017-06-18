using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Notification.Abstract;
using System.Net.Mail;

namespace Funkmap.Common.Notification
{
    public class EmailNotificationService : INotificationService
    {
        public async Task<bool> SendNotification(Notification notification)
        {
            await Task.Yield();
            try
            {
                var appEmail = ConfigurationManager.AppSettings["email"];
                if (String.IsNullOrEmpty(appEmail))
                {
                    throw new InvalidOperationException("Необходимо записать почту приложения в app.config");
                }
                
                var appEmailPassword = ConfigurationManager.AppSettings["emailPassword"];
                if (String.IsNullOrEmpty(appEmailPassword))
                {
                    throw new InvalidOperationException("Необходимо записать пароль от почты приложения в app.config");
                }

                var message = new MailMessage();
                message.From = new MailAddress(appEmail);
                message.To.Add(new MailAddress(notification.Receiver));
                message.Body = notification.Body;
                message.Subject = notification.Subject;

                var smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 25;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(appEmail, appEmailPassword);
                smtpClient.Send(message);
                message.Dispose();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
    }
}
