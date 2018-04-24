namespace Funkmap.Domain.Abstract
{
    public interface ICommandParameter<T> : IAuthCommandParameter
    {
        T Parameter { get; set; }
    }

    public interface IAuthCommandParameter
    {
        string UserLogin { get; set; }
    }
}
