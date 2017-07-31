
using System.Collections.Generic;

namespace Funkmap.Messenger.Services
{
    public interface IMessengerCacheService
    {
        void AddOnlineUser(string id, string login);
        void RemoveOnlineUser(string id);

        ICollection<string> GetConnectionIdsByLogin(string login);
    }
}
