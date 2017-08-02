
using System.Collections.Generic;

namespace Funkmap.Messenger.Services
{
    public interface IMessengerCacheService
    {
        void AddOnlineUser(string id, string login);
        void RemoveOnlineUser(string id, out string login);

        ICollection<string> GetConnectionIdsByLogins(ICollection<string> login);

        ICollection<string> GetOnlineUsersLogins();
    }
}
