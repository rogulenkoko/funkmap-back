using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Redis.Abstract;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Specific;
using Funkmap.Notifications.Contracts.Specific.BandInvite;

namespace Funkmap.Services.Abstract
{
    public interface IFunkmapNotificationService : IEventHandler<NotificationAnswer>
    {
        void NotifyBandInvite(BandInviteNotification notification);
    }
}
