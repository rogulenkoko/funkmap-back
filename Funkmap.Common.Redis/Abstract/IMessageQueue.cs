using System;
using System.Threading.Tasks;

namespace Funkmap.Common.Redis.Abstract
{
    public interface IMessageQueue
    {
        Task PublishAsync(object value);
        void Subscribe<T>(Action<T> handler) where T : class;
    }
}
