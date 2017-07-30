using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Messenger.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Funkmap.Messenger
{
    [HubName("messenger")]
    public class MessengerHub : Hub, IMessengerHub
    {
        private readonly IMessengerCacheService _cacheService;

        public MessengerHub(IMessengerCacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public void Connect(string login)
        {
            Clients.All.onUserConnected(login);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
    }
}
