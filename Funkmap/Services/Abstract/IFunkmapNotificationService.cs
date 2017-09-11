using Funkmap.Models.Requests;
using Funkmap.Notifications.Contracts.Funkmap;

namespace Funkmap.Services.Abstract
{
    public interface IFunkmapNotificationService
    {
        void InviteMusicianToGroup(InviteToGroupRequest request);
    }
}
