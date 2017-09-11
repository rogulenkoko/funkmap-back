using Funkmap.Common.RedisMq;
using Funkmap.Notifications.Contracts.Funkmap;
using Funkmap.Services.Abstract;
using ServiceStack.Messaging;

namespace Funkmap.Services
{
    public class FunkmapNotificationService : RedisMqProducer, IRedisMqConsumer, IFunkmapNotificationService
    {
        private readonly IMessageService _messageService;
        public FunkmapNotificationService(IMessageFactory redisMqFactory, IMessageService messageService) : base(redisMqFactory)
        {
            _messageService = messageService;
        }

        public void InitHandlers()
        {
            _messageService.RegisterHandler<InviteToGroupBack>(request=> OnGroupInviteAnswered(request?.GetBody()));
        }

        private bool OnGroupInviteAnswered(InviteToGroupBack request)
        {
            return true;
        }

        public void InviteMusicianToGroup(InviteToGroupRequest request)
        {
            Publish<InviteToGroupRequest>(request);
        }
    }
}
