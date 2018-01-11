using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Command.Handlers;
using Funkmap.Messenger.Command.Repositories;

namespace Funkmap.Messenger.Command
{
    public class MessengerCommandModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MessengerCommandRepository>().As<IMessengerCommandRepository>();
            builder.RegisterType<DialogLastMessageCommandHandler>().As<ICommandHandler<UpdateDialogLastMessageCommand>>();
            builder.RegisterType<SaveMessageCommandHandler>().As<ICommandHandler<SaveMessageCommand>>();
            builder.RegisterType<ReadMessagesCommandHandler>().As<ICommandHandler<ReadMessagesCommand>>();
        }
    }
}
