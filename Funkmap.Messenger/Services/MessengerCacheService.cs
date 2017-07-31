using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Funkmap.Messenger.Services
{
    public class MessengerCacheService : IMessengerCacheService
    {
        private readonly ConcurrentDictionary<string, string> _onlineUsers;

        public MessengerCacheService()
        {
            _onlineUsers = new ConcurrentDictionary<string, string>();
        }


        public void AddOnlineUser(string id, string login)
        {
            RemoveOnlineUser(id); //на всякий
            _onlineUsers[id] = login;
        }

        public void RemoveOnlineUser(string id)
        {
            if (_onlineUsers.ContainsKey(id))
            {
                string login;
                _onlineUsers.TryRemove(id, out login);
            }
        }

        public ICollection<string> GetConnectionIdsByLogin(string login)
        {
            return _onlineUsers.Where(x => x.Value == login).Select(x => x.Key).ToList();
        }
    }
}
