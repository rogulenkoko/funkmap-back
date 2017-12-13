using System.Configuration;
using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Redis.Abstract;
using StackExchange.Redis;

namespace Funkmap.Common.Redis.Autofac
{
    public class RedisModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["redis-primary"]);

            builder.RegisterInstance(redis).As<ConnectionMultiplexer>().SingleInstance().OnRelease(x => x.Dispose());
            builder.Register(container =>
                {
                    IDatabase db = redis.GetDatabase(1, asyncState: true);
                    return db;

                })
                .As<IDatabase>()
                .SingleInstance();

            builder.RegisterType<NewtonSerializer>().As<ISerializer>();
            builder.RegisterType<RedisStorage>().As<IStorage>().SingleInstance();

            builder.Register(container =>
                {
                    ISubscriber sub = redis.GetSubscriber();
                    return sub;
                })
                .As<ISubscriber>()
                .SingleInstance();

            builder.RegisterType<RedisMessageQueue>().As<IMessageQueue>().SingleInstance();
        }
    }
}
