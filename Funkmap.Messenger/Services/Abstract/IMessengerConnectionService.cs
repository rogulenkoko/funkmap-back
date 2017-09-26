
using Funkmap.Common.SignalR.Abstract;

namespace Funkmap.Messenger.Services
{
    public interface IMessengerConnectionService : IConnectionService
    {

        bool CheckDialogIsOpened(string login, string dialogId);
        bool SetOpenedDialog(string id, string dialogId);
    }
}
