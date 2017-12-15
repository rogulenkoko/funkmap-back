using System.Threading.Tasks;
using Funkmap.Notifications.Models;

namespace Funkmap.Notifications.Hubs
{
    public interface INotificationsHub
    {
        Task OnNotificationRecieved(NotificationModel notification);
    }
}
