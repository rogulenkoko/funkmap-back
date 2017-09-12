using System;

namespace Funkmap.Notifications.Contracts
{
    public interface INotificationTypesPair
    {
        Type RequestType { get; }
        Type ResponseType { get; }
    }
}
