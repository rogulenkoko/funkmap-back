using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Messenger.Models;

namespace Funkmap.Messenger.Services
{
    public class MessengerCacheService : IMessengerCacheService
    {
        private readonly ConcurrentDictionary<string, UserCached> _onlineUsers;

        public MessengerCacheService()
        {
            _onlineUsers = new ConcurrentDictionary<string, UserCached>();
        }


        public void AddOnlineUser(string id, string login)
        {
            _onlineUsers[id] = new UserCached()
            {
                Login = login
            };
        }

        public void RemoveOnlineUser(string id, out string login)
        {
            if (_onlineUsers.ContainsKey(id))
            {
                UserCached user;
                _onlineUsers.TryRemove(id, out user);
                var removedLogin = user.Login;
                login = user.Login;
                var removedLoginKey = _onlineUsers.Where(x => x.Value.Login == removedLogin).Select(x=>x.Key).ToList();
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

        public ICollection<string> GetConnectionIdsByLogins(ICollection<string> logins)
        {
            return _onlineUsers.Where(x => logins.Contains(x.Value.Login)).Select(x => x.Key).ToList();
        }

        public ICollection<string> GetOnlineUsersLogins()
        {
            return _onlineUsers.Values.Select(x=>x.Login).Distinct().ToList();
        }

        public bool CheckDialogIsOpened(string login, string dialogId)
        {
            return _onlineUsers.Where(x => x.Value.Login == login)
                .Select(x => x.Value.OpenedDialogId)
                .Contains(dialogId);
        }

        public bool SetOpenedDialog(string id, string dialogId)
        {
            if (_onlineUsers.ContainsKey(id) && _onlineUsers[id] != null)
            {
                _onlineUsers[id].OpenedDialogId = dialogId;
                return true;
            }
            return false;
        }
    }
}
