using System.Threading.Tasks;
using Funkmap.Common.Models;
using Funkmap.Messenger.Models;

namespace Funkmap.Messenger.Hubs
{
    public interface IMessengerHub
    {
        Task<BaseResponse> SendMessage(Message message);
    }
}
