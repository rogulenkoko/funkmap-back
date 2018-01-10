using Autofac;
using Funkmap.Common.Cqrs;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Command.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Messenger.Tests.Commands
{
    [TestClass]
    public class SaveMessageCommandTest
    {

        private IContainer _container;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<InMemoryCommandBus>().As<ICommandBus>();
            builder.RegisterType<CommandHandlerResolver>().As<ICommandHandlerResolver>();

            _container = builder.Build();
        }

        [TestMethod]
        public void SaveMessagePositiveTest()
        {
            ICommandBus commandBus = _container.Resolve<ICommandBus>();



            var command = new SaveMessageCommand()
            {
                Sender = "rogulenkoko",
                DialogId = "5a566b36208c113d284a5070",
                Text = "hihihi"
            };

            commandBus.Execute<SaveMessageCommand>(command).GetAwaiter().GetResult();
        }
    }
}
