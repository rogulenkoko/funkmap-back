using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Specific.BandInvite;

namespace Funkmap.Domain.Services.Abstract
{
    public interface IFunkmapNotificationService : IEventHandler<NotificationAnswer>
    {
        void NotifyBandInvite(BandInviteNotification notification);
    }
}
