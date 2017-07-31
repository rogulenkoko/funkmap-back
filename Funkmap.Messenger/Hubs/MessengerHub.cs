using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Models;
using Funkmap.Messenger.Models;
using Funkmap.Messenger.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Funkmap.Messenger.Hubs
{
    [HubName("messenger")]
    public class MessengerHub : Hub, IMessengerHub
    {
        private readonly IMessengerCacheService _cacheService;

        public MessengerHub(IMessengerCacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HubMethodName("sendMessage")]
        public BaseResponse SendMessage(Message message)
        {
            var receiverConnectionIds = _cacheService.GetConnectionIdsByLogin(message.Receiver);
            var senderConnectionIds = _cacheService.GetConnectionIdsByLogin(message.Sender);
            var allIds = receiverConnectionIds.Concat(senderConnectionIds).ToList();
            Clients.Clients(allIds).OnMessageSent(message);
            return new BaseResponse() {Success = true};
        } 

        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            var login = Context.QueryString["login"];

            _cacheService.AddOnlineUser(connectionId, login);

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            
            var connectionId = Context.ConnectionId;
            _cacheService.RemoveOnlineUser(connectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}
