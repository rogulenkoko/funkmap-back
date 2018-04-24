
using System.Collections.Generic;
using Funkmap.Common.SignalR.Abstract;

namespace Funkmap.Messenger.Services.Abstract
{
    public interface IMessengerConnectionService : IConnectionService
    {

        bool CheckDialogIsOpened(string login, string dialogId);
        bool SetOpenedDialog(string id, string dialogId);

        List<string> GetDialogParticipants(string dialogId);
    }
}
