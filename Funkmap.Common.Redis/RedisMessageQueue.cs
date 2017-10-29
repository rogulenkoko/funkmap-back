using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Redis.Abstract;
using Funkmap.Common.Redis.Options;
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

        public async Task PublishAsync(object value, MessageQueueOptions options = null)
        {
            if(value == null) throw new ArgumentNullException(nameof(value));

            var key = value.GetType().FullName;
            
            await PublishAsync(key, value, options);
        }

        public async Task PublishAsync(string key, object value, MessageQueueOptions options = null)
        {
            if (options?.SpecificKey != null)
            {
                var specificKeyString = _serializer.Serialize(options.SpecificKey);
                key = $"{key}_{specificKeyString}";
            }
            string serialized = _serializer.Serialize(value);
            await _subscriber.PublishAsync(key, serialized);
        }

        public void Subscribe<T>(Action<T> handler, MessageQueueOptions options = null) where T : class 
        {
            var type = typeof(T);

            if (_registeredTypes.Contains(type))
            {
                throw new InvalidOperationException("type has already been registered");
            }

            _registeredTypes.Add(type);

            var key = type.FullName;
            Subscribe<T>(key, handler, options);
            
        }

        public void Subscribe<T>(string key, Action<T> handler, MessageQueueOptions options = null) where T : class
        {
            if (options?.SpecificKey != null)
            {
                var specificKeyString = _serializer.Serialize(options.SpecificKey);
                key = $"{key}_{specificKeyString}";
            }

            _subscriber.Subscribe(key, (channel, value) =>
            {
                var deserialized = _serializer.Deserialize<T>(value);
                handler.Invoke(deserialized);
            });
        }
    }
}
