using System.Collections.Generic;

namespace Funkmap.Common.SignalR.Abstract
{
    public interface IConnectionService
    {
        void AddOnlineUser(string id, string login);
        void RemoveOnlineUser(string id, out string login);

        List<string> GetConnectionIdsByLogins(ICollection<string> login);
        List<string> GetConnectionIdsByLogin(string login);
        List<string> GetOnlineUsersLogins();

        string GetLoginByConnectionId(string connectionId);
    }
}
