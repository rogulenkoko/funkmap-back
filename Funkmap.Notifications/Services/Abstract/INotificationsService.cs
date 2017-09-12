using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Notifications.Contracts;

namespace Funkmap.Notifications.Services.Abstract
{
    public interface INotificationsService
    {
        NotificationType NotificationType { get; }

        //Task<ICollection<NotificationModel>> GetNotifications(string login);

        void PublishBackRequest(NotificationBack request);
    }

   
}
