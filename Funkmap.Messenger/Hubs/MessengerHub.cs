using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Models;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Messenger.Mappers;
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
        private readonly IMessageRepository _messageRepository;

        public MessengerHub(IMessengerCacheService cacheService,
                            IMessageRepository messageRepository)
        {
            _cacheService = cacheService;
            _messageRepository = messageRepository;
        }

        [HubMethodName("sendMessage")]
        public async Task<BaseResponse> SendMessage(Message message)
        {
            var response = new BaseResponse();

            var receiverConnectionIds = _cacheService.GetConnectionIdsByLogin(message.Receiver);
            var senderConnectionIds = _cacheService.GetConnectionIdsByLogin(message.Sender);
            var allIds = receiverConnectionIds.Concat(senderConnectionIds).ToList();

            try
            {
                await _messageRepository.CreateAsync(message.ToEntity());
                Clients.Clients(allIds).OnMessageSent(message);
                response.Success = true;
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BaseResponse();
            }
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
