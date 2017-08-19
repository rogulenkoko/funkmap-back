using Autofac;
using Funkmap.Common.Abstract;
using ServiceStack.Messaging;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;

namespace Funkmap.Middleware.Module
{
    public class RedisMqModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            var redisHost = "localhost:6379";
            IRedisClientsManager redisClientManager = new PooledRedisClientManager(redisHost);

            IMessageFactory redisMqFactory = new RedisMessageFactory(redisClientManager);
            builder.RegisterInstance(redisMqFactory).As<IMessageFactory>().SingleInstance();

            IMessageService redisMqServer = new RedisMqServer(redisClientManager, retryCount: 2);
            builder.RegisterInstance(redisMqServer).As<IMessageService>().SingleInstance().OnActivating(x=> x.Instance.Start());
        }
    }
}
