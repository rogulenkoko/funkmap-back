using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.QueryExecutors;
using Funkmap.Messenger.Query.Responses;

namespace Funkmap.Messenger.Query
{
    public class MessengerQueryModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<UserDialogsQueryExecutor>().As<IQueryExecutor<UserDialogsQuery, UserDialogsResponse>>();
            builder.RegisterType<DialogMessagesQueryExecutor>().As<IQueryExecutor<DialogMessagesQuery, DialogMessagesResponse>>();
        }
    }
}
