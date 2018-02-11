using System;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs;
using Funkmap.Common.Cqrs.Abstract;
using StackExchange.Redis;

namespace Funkmap.Common.Redis
{
    public class RedisEventBus : IEventBus
    { 

        private readonly ISubscriber _subscriber;
        private readonly ISerializer _serializer;

        public RedisEventBus(ISubscriber subscriber, ISerializer serializer)
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
                var specificKeyString = _serializer.Serialize(options.SpecificKey, options?.SerializerOptions);
                key = $"{key}_{specificKeyString}";
            }
            string serialized = _serializer.Serialize(value, options?.SerializerOptions);
            await _subscriber.PublishAsync(key, serialized);
        }

        public void Subscribe<T>(Func<T, Task> handler, MessageQueueOptions options = null) where T : class 
        {
            var type = typeof(T);

            var key = type.FullName;
            Subscribe<T>(key, handler, options);
            
        }

        public void Subscribe<T>(string key, Func<T, Task> handler, MessageQueueOptions options = null) where T : class
        {
            if (options?.SpecificKey != null)
            {
                var specificKeyString = _serializer.Serialize(options.SpecificKey, options?.SerializerOptions);
                key = $"{key}_{specificKeyString}";
            }

            _subscriber.Subscribe(key, (channel, value) =>
            {
                var deserialized = _serializer.Deserialize<T>(value, options?.SerializerOptions);
                handler.Invoke(deserialized);
            });
        }
    }
}
