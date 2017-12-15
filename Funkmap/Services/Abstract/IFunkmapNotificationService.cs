using Funkmap.Common.Redis.Abstract;
using Funkmap.Notifications.Contracts.Specific;
using Funkmap.Notifications.Contracts.Specific.BandInvite;

namespace Funkmap.Services.Abstract
{
    public interface IFunkmapNotificationService : IMessageHandler
    {
        void NotifyBandInvite(BandInviteNotification notification);
    }
}
