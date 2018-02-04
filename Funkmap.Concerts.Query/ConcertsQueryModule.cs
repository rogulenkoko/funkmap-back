using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Concerts.Query.Queries;
using Funkmap.Concerts.Query.QueryExecutors;
using Funkmap.Concerts.Query.Responses;

namespace Funkmap.Concerts.Query
{
    public class ConcertsQueryModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<ActiveQueryExecutor>().As<IQueryExecutor<ActiveQuery, ActiveResponse>>();
            builder.RegisterType<NotFinishedQueryExecutor>().As<IQueryExecutor<NotFinishedQuery, NotFinishedConcertResponse>>();
        }
    }
}
