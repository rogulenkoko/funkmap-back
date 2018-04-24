
namespace Funkmap.Domain.Abstract
{
    public interface ICommandResponse
    {
        bool Success { get; }

        string Error { get; }
    }

    public interface ICommandResponse<T> : ICommandResponse
    {
        T Body { get; }
    }

    public class CommandResponse : ICommandResponse
    {
        public CommandResponse(bool succes)
        {
            Success = succes;
        }
        public bool Success { get; }

        public string Error { get; set; }
    }

    public class CommandResponse<T> : CommandResponse, ICommandResponse<T>
    {
        public T Body { get; set; }

        public CommandResponse(bool succes) : base(succes)
        {
        }
    }
}
