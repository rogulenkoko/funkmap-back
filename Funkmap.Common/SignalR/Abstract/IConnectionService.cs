using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Common.SignalR.Abstract
{
    public interface IConnectionService
    {
        void AddOnlineUser(string id, string login);
        void RemoveOnlineUser(string id, out string login);

        ICollection<string> GetConnectionIdsByLogins(ICollection<string> login);
        ICollection<string> GetOnlineUsersLogins();
    }
}
