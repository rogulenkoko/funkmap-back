using System.Threading.Tasks;

namespace Funkmap.Common.Cqrs.Abstract
{
    public interface ICommandBus
    {
        Task ExecuteEnvelope<TCommand>(Envelope<TCommand> command) where TCommand : class;
        Task Execute<TCommand>(TCommand command) where TCommand : class;
    }
}
