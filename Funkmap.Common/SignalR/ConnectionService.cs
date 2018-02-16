using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Common.SignalR.Abstract;

namespace Funkmap.Common.SignalR
{
    public class ConnectionService : IConnectionService
    {
        protected readonly ConcurrentDictionary<string, UserConnection> _onlineUsers;


        public ConnectionService()
        {
            _onlineUsers = new ConcurrentDictionary<string, UserConnection>();
        }

        public void AddOnlineUser(string id, string login)
        {
            _onlineUsers[id] = new UserConnection()
            {
                Login = login
            };
        }

        public void RemoveOnlineUser(string id, out string login)
        {
            if (_onlineUsers.ContainsKey(id))
            {
                UserConnection user;
                _onlineUsers.TryRemove(id, out user);
                var removedLogin = user.Login;
                login = user.Login;
                var removedLoginKey = _onlineUsers.Where(x => x.Value.Login == removedLogin).Select(x => x.Key).ToList();
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

        public List<string> GetConnectionIdsByLogins(ICollection<string> logins)
        {
            if(logins == null) return new List<string>();
            return _onlineUsers.Where(x => logins.Contains(x.Value.Login)).Select(x => x.Key).ToList();
        }

        public List<string> GetConnectionIdsByLogin(string login)
        {
            return _onlineUsers.Where(x => x.Value.Login == login).Select(x => x.Key).ToList();
        }

        public List<string> GetOnlineUsersLogins()
        {
            return _onlineUsers.Values.Select(x => x.Login).Distinct().ToList();
        }

        public string GetLoginByConnectionId(string connectionId)
        {
            return !_onlineUsers.ContainsKey(connectionId) ? String.Empty : _onlineUsers[connectionId].Login;
        }
    }
}
