using System.Collections.Generic;
using Funkmap.Notifications.Contracts;

namespace Funkmap.Notifications.Services.Abstract
{
    public interface INotificationsService
    {
        NotificationType NotificationType { get; }

        ICollection<Notification> GetNotifications(string login);

        void PublishBackRequest(NotificationBack request);
    }

   
}
