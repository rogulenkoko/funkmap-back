using Funkmap.Common.Redis.Abstract;
using Funkmap.Notifications.Contracts.Specific;

namespace Funkmap.Services.Abstract
{
    public interface IFunkmapNotificationService : IMessageHandler
    {
        void NotifyBandInvite(BandInviteNotification notification);
    }
}
