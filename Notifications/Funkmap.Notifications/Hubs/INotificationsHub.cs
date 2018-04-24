using System.Threading.Tasks;
using Funkmap.Notifications.Domain.Models;

namespace Funkmap.Notifications.Hubs
{
    public interface INotificationsHub
    {
        Task OnNotificationRecieved(Notification notification);
    }
}
