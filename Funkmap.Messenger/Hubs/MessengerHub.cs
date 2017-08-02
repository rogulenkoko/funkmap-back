using System;
using System.Collections.Generic;
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
        private readonly IDialogRepository _dialogRepository;

        public MessengerHub(IMessengerCacheService cacheService,
                            IDialogRepository dialogRepository)
        {
            _cacheService = cacheService;
            _dialogRepository = dialogRepository;
        }

        [HubMethodName("sendMessage")]
        public async Task<BaseResponse> SendMessage(Message message)
        {
            var response = new BaseResponse();

            var members = await _dialogRepository.GetDialogMembers(message.DialogId);
            var clientIds = _cacheService.GetConnectionIdsByLogins(members).ToList();

            try
            {
                await _dialogRepository.AddMessage(message.DialogId, message.ToEntity());
                Clients.Clients(clientIds).OnMessageSent(message);
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

            Clients.All.onUserConnected(login);

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            
            var connectionId = Context.ConnectionId;
            string login;
            _cacheService.RemoveOnlineUser(connectionId, out login);
            Clients.All.onUserDisconnected(login);

            return base.OnDisconnected(stopCalled);
        }
    }
}
