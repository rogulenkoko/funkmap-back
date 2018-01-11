using System.Threading.Tasks;

namespace Funkmap.Common.Cqrs.Abstract
{
    public interface IQueryContext
    {
        Task<TResponse> Execute<TQuery, TResponse>(TQuery query) where TQuery : class where TResponse : class;
    }
}
