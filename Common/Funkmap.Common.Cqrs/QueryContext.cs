using System;
using System.Threading.Tasks;
using Autofac;
using Funkmap.Common.Cqrs.Abstract;

namespace Funkmap.Common.Cqrs
{
    public class QueryContext : IQueryContext
    {
        private readonly IComponentContext _componentContext;

        public QueryContext(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public async Task<TResponse> ExecuteAsync<TQuery, TResponse>(TQuery query) where TQuery : class where TResponse : class
        {
            if (!_componentContext.IsRegistered<IQueryExecutor<TQuery, TResponse>>())
            {
                throw new InvalidOperationException("QueryExecutor for this response and query is not registered");
            }

            var executor = _componentContext.Resolve<IQueryExecutor<TQuery, TResponse>>();
            var response = await executor.Execute(query);
            return response;
        }
    }
}
