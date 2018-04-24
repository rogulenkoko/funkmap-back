using System.Threading.Tasks;

namespace Funkmap.Common.Cqrs.Abstract
{
    public interface IQueryExecutor<in TQuery, TResponse> where TQuery : class where TResponse: class
    {
        Task<TResponse> Execute(TQuery query);
    }
}
