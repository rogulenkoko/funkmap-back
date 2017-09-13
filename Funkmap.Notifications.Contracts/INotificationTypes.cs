using System;

namespace Funkmap.Notifications.Contracts
{
    public interface INotificationTypes
    {
        Type RequestType { get; }
        Type ResponseType { get; }

        NotificationType NotificationType { get; }
    }
}
