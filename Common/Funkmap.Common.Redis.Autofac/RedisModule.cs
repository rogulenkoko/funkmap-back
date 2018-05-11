using Autofac;
using StackExchange.Redis;
using System.Configuration;

namespace Funkmap.Common.Redis.Autofac
{
    public class RedisModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(container =>
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["redis-primary"]);
                return redis;
            }).As<ConnectionMultiplexer>().SingleInstance().OnRelease(x => x.Dispose());

            builder.Register(container =>
                {
                    IDatabase db = container.Resolve<ConnectionMultiplexer>().GetDatabase(1, asyncState: true);
                    return db;

                })
                .As<IDatabase>()
                .SingleInstance();


            builder.Register(container =>
                    {
                        ISubscriber sub = container.Resolve<ConnectionMultiplexer>().GetSubscriber();
                        return sub;
                    })
                    .As<ISubscriber>()
                    .SingleInstance();
        }
    }
}
