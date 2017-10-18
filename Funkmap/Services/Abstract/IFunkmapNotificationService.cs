using Funkmap.Common.Redis.Abstract;
using Funkmap.Contracts.Notifications;

namespace Funkmap.Services.Abstract
{
    public interface IFunkmapNotificationService : IMessageHandler
    {
        void InviteMusicianToGroup(InviteToBandRequest request);
    }
}
