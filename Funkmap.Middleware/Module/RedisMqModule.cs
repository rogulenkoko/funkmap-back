using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.RedisMq;
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
            builder.RegisterInstance(redisMqServer).As<IMessageService>().SingleInstance();

            var modules = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("Funkmap"))
                .SelectMany(s => s.GetTypes())
                .Where(x => x.GetInterfaces().Contains(typeof(IRedisMqConsumer)))
                .Distinct()
                .ToList();
            builder.RegisterTypes(modules.ToArray()).As<IRedisMqConsumer>();

            builder.RegisterType<RedisMqModulesActivator>().AsSelf().AutoActivate();

        }
    }


    public class RedisMqModulesActivator
    {
        public RedisMqModulesActivator(IEnumerable<IRedisMqConsumer> services, IMessageService redisMqServer)
        {
            foreach (var service in services)
            {
                service.InitHandlers();
            }

            redisMqServer.Start();
        }
    }
}
