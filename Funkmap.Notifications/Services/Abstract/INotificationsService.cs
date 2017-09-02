using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Notifications.Models;

namespace Funkmap.Notifications.Services.Abstract
{
    public interface INotificationsService
    {
        ICollection<Notification> GetNotifications(string login);
    }
}
