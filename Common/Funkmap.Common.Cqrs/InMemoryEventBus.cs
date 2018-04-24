using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;

namespace Funkmap.Common.Cqrs
{
    public class InMemoryEventBus : IEventBus
    {
        private static readonly MultiValueDictionary<string, Action<object>> _handlers = new MultiValueDictionary<string, Action<object>>();
        
        public async Task PublishAsync(object value, MessageQueueOptions options = null)
        {
            if(value == null) throw new ArgumentNullException(nameof(value));
            var key = value.GetType().FullName;
            await PublishAsync(key, value, options);
        }

        public async Task PublishAsync(string key, object value, MessageQueueOptions options = null)
        {
            await Task.Yield();
            if (options?.SpecificKey != null)
            {
                key = $"{key}_{options.SpecificKey}";
            }

            IReadOnlyCollection<Action<object>> handlers;
            _handlers.TryGetValue(key, out handlers);

            if(handlers == null) return;

            foreach (var handler in handlers)
            {
                handler.Invoke(value);
            }
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
                key = $"{key}_{options.SpecificKey}";
            }

            _handlers.Add(key, obj => handler.Invoke((T)obj));

        }
    }
}
