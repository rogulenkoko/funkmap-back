using System;
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
            _onlineUsers[id] = login;
        }

        public void RemoveOnlineUser(string id, out string login)
        {
            if (_onlineUsers.ContainsKey(id))
            {
                _onlineUsers.TryRemove(id, out login);
                var removedLogin = login;

                var removedLoginKey = _onlineUsers.Where(x => x.Value == removedLogin).Select(x=>x.Key).ToList();
                if (removedLoginKey.Count > 0)
                {
                    login = String.Empty;
                }
            }
            else
            {
                login = String.Empty;
            }
            
        }

        public ICollection<string> GetConnectionIdsByLogin(string login)
        {
            return _onlineUsers.Where(x => x.Value == login).Select(x => x.Key).ToList();
        }

        public ICollection<string> GetOnlineUsersLogins()
        {
            return _onlineUsers.Values.Distinct().ToList();
        }
    }
}
