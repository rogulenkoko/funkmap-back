using System.Diagnostics;
using System.Threading;
using Autofac;
using Funkmap.Common.Redis.Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;

namespace Funkmap.Common.Tests.Redis
{
    [TestClass]
    public class RedisTest
    {
        private IContainer _container;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ContainerBuilder();
            new RedisModule().Register(builder);
            _container = builder.Build();
        }

        [TestMethod]
        public void PubSubTest()
        {
            var subscriber = _container.Resolve<ISubscriber>();

            var channel = "test";

            subscriber.Subscribe(channel, (redisChannel, value) =>
            {
                Trace.WriteLine(value);
            });

            subscriber.Subscribe(channel, (redisChannel, value) =>
            {
                Trace.WriteLine($"-------{value}------");
            });
            
            subscriber.Publish(channel, "qweqwe");

            Thread.Sleep(10000);
        }
    }
}
