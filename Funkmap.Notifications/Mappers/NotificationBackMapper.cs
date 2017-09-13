using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Contracts.Notifications;
using Funkmap.Notifications.Contracts;

namespace Funkmap.Notifications.Mappers
{
    public static class NotificationBackMapper
    {
        public static NotificationBack ToSpecificNotificationBack(this NotificationBack source, NotificationType type)
        {
            switch (type)
            {
                    case NotificationType.BandInvite:
                    return new InviteToBandBack()
                    {
                        Notification = source.Notification,
                        Answer = source.Answer
                    };
                default: return null;
            }
        }
    }
}
