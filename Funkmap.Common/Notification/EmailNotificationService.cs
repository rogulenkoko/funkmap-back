using System;
using System.Collections.Generic;
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
                var message = new MailMessage();
                message.From = new MailAddress("rogulenkoko@gmail.com");
                message.To.Add(new MailAddress(notification.Receiver));
                message.Body = notification.Body;
                message.Subject = notification.Subject;

                var smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 25;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential("rogulenkoko@gmail.com", "6278476q");
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
