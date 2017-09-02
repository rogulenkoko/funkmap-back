using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Notifications.Models;
using Funkmap.Notifications.Services.Abstract;

namespace Funkmap.Notifications.Services
{
    public class NotificationsService : INotificationsService
    {
        public ICollection<Notification> GetNotifications(string login)
        {
            throw new NotImplementedException();
        }
    }
}
