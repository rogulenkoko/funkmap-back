using System.Threading.Tasks;
using Funkmap.Messenger.Models;

namespace Funkmap.Messenger.Hubs.Abstract
{
    public interface IMessengerHub
    {
        Task OnUserConnected(string userLogin);
        Task OnUserDisconnected(string userLogin);
        Task OnMessageSent(Message message);

        Task OnDialogRead(string dialogId);
    }
}
