using System.Threading.Tasks;

namespace Funkmap.Common.Cqrs.Abstract
{
    public interface IEventHandler
    {
        void InitHandlers();
    }

    public interface IEventHandler<in T> : IEventHandler where T : class
    {
        void Handle(T @event);
    }
}
