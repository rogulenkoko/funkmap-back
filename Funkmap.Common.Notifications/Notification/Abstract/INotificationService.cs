using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Common.Notifications.Notification.Abstract
{
    public interface INotificationService
    {
        Task<bool> SendNotification(Notification notification);
    }
}
