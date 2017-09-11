using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Auth.Contracts;
using ServiceStack.Messaging;

namespace Funkmap.Common.RedisMq
{
    
    public abstract class RedisMqProducer
    {
        protected readonly IMessageFactory _redisMqFactory;

        protected RedisMqProducer(IMessageFactory redisMqFactory)
        {
            _redisMqFactory = redisMqFactory;
        }

        protected virtual TResponse GetResponse<TResponse, TRequest>(TRequest request, TimeSpan? responseTimeOut = null) where TResponse : class
            where TRequest : class
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            using (var mqClient = _redisMqFactory.CreateMessageQueueClient())
            {
                var messengerMqName = QNameBuilder.BuildQueueName();
                mqClient.Publish(new Message<TRequest>(request)
                {
                    ReplyTo = messengerMqName
                });

                var response = mqClient.Get<TResponse>(messengerMqName, responseTimeOut);

                return response?.GetBody();
            }
        }

        protected virtual void Publish<TRequest>(TRequest request) where TRequest : class
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            using (var mqClient = _redisMqFactory.CreateMessageQueueClient())
            {
                mqClient.Publish(new Message<TRequest>(request));
            }
        }
    }
}
