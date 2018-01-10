using System.Collections.Generic;

namespace Funkmap.Common.SignalR.Abstract
{
    public interface IConnectionService
    {
        void AddOnlineUser(string id, string login);
        void RemoveOnlineUser(string id, out string login);

        ICollection<string> GetConnectionIdsByLogins(ICollection<string> login);
        ICollection<string> GetConnectionIdsByLogin(string login);
        ICollection<string> GetOnlineUsersLogins();

        string GetLoginByConnectionId(string connectionId);
    }
}
