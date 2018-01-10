using System.Collections.Generic;
using System.Linq;
using Funkmap.Common.SignalR;
using Funkmap.Messenger.Services.Abstract;

namespace Funkmap.Messenger.Services
{
    public class MessengerConnectionService : ConnectionService, IMessengerConnectionService
    {
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

        public ICollection<string> GetDialogParticipants(string dialogId)
        {
            return _onlineUsers.Where(x => x.Value.OpenedDialogId == dialogId).Select(x => x.Value.Login).ToList();
        }
    }
}
