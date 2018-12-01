using System;
using System.Threading.Tasks;
using Funkmap.Notifications.Domain.Services.Abstract;
using Microsoft.AspNetCore.SignalR;

namespace Funkmap.Notifications.Hubs
{
    /// <inheritdoc cref="INotificationsHub"/>
    public class NotificationsHub : Hub
    {
        private readonly INotificationsConnectionService _connectionService;

        public NotificationsHub(INotificationsConnectionService connectionService)
        {
            _connectionService = connectionService;
        }
        
        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var login = (string)Context.Items["login"];
            _connectionService.AddOnlineUser(connectionId, login);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            _connectionService.RemoveOnlineUser(connectionId, out var login);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
