using Funkmap.Domain.Abstract;

namespace Funkmap.Domain.Parameters
{
    public class CommandParameter<T> : ICommandParameter<T>
    {
        public string UserLogin { get; set; }
        public T Parameter { get; set; }
    }
}
