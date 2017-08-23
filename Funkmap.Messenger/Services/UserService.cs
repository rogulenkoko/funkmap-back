using System;
using System.Threading.Tasks;
using Funkmap.Auth.Contracts;
using Funkmap.Auth.Contracts.Models;
using Funkmap.Auth.Contracts.Services;
using ServiceStack.Messaging;

namespace Funkmap.Messenger.Services
{
    public class UserService : IUserMqService
    {

        private readonly IMessageFactory _redisMqFactory;
        
        private readonly TimeSpan _mqTimeOut = TimeSpan.FromSeconds(15);

        public UserService(IMessageFactory redisMqFactory)
        {
            _redisMqFactory = redisMqFactory;
        }

        public UserUpdateLastVisitDateResponse UpdateLastVisitDate(UserUpdateLastVisitDateRequest request)
        {
            using (var mqClient = _redisMqFactory.CreateMessageQueueClient())
            {
                var messengerMQName = QNameBuilder.BuildQueueName("messenger");
                mqClient.Publish(new Message<UserUpdateLastVisitDateRequest>(request)
                {
                    ReplyTo = messengerMQName
                });

                var response = mqClient.Get<UserUpdateLastVisitDateResponse>(messengerMQName, _mqTimeOut).GetBody();
                return response;
            }
        }
    }
}
