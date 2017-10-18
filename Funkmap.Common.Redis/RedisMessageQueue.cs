using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Redis.Abstract;
using StackExchange.Redis;

namespace Funkmap.Common.Redis
{
    public class RedisMessageQueue : IMessageQueue
    {
        private readonly HashSet<Type> _registeredTypes = new HashSet<Type>();

        private readonly ISubscriber _subscriber;
        private readonly ISerializer _serializer;

        public RedisMessageQueue(ISubscriber subscriber, ISerializer serializer)
        {
            _subscriber = subscriber;
            _serializer = serializer;
        }

        public async Task PublishAsync(object value)
        {
            var key = value.GetType().FullName;
            string serialized = _serializer.Serialize(value);
            await _subscriber.PublishAsync(key, serialized);
        }

        public void Subscribe<T>(Action<T> handler) where T : class 
        {
            var type = typeof(T);

            if (_registeredTypes.Contains(type))
            {
                throw new InvalidOperationException("type has already been registered");
            }

            _registeredTypes.Add(type);

            var key = type.FullName;

            _subscriber.Subscribe(key, (channel, value) =>
            {
                var deserialized = _serializer.Deserialize<T>(value);
                handler.Invoke(deserialized);
            });
        }
    }
}
