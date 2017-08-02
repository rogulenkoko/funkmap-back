
using System.Collections.Generic;

namespace Funkmap.Messenger.Services
{
    public interface IMessengerCacheService
    {
        void AddOnlineUser(string id, string login);
        void RemoveOnlineUser(string id, out string login);

        ICollection<string> GetConnectionIdsByLogin(string login);

        ICollection<string> GetOnlineUsersLogins();
    }
}
