using Funkmap.Contracts.Notifications;

namespace Funkmap.Services.Abstract
{
    public interface IFunkmapNotificationService
    {
        void InviteMusicianToGroup(InviteToBandRequest request);
    }
}
