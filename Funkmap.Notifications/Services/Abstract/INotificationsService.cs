using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Notifications.Contracts;

namespace Funkmap.Notifications.Services.Abstract
{
    public interface INotificationsService
    {
        NotificationType NotificationType { get; }

        void PublishBackRequest(NotificationBack request);
    }

   
}
