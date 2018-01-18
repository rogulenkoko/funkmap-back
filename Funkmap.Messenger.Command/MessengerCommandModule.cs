using Autofac;
using Autofac.Features.AttributeFilters;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Command.Abstract;
using Funkmap.Messenger.Command.CommandHandlers;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Command.Repositories;

namespace Funkmap.Messenger.Command
{
    public class MessengerCommandModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MessengerCommandRepository>().As<IMessengerCommandRepository>().WithAttributeFiltering();
            builder.RegisterType<DialogLastMessageCommandHandler>().As<ICommandHandler<UpdateDialogLastMessageCommand>>();
            builder.RegisterType<SaveMessageCommandHandler>().As<ICommandHandler<SaveMessageCommand>>(); 
            builder.RegisterType<ReadMessagesCommandHandler>().As<ICommandHandler<ReadMessagesCommand>>(); 
            builder.RegisterType<CreateDialogCommandHandler>().As<ICommandHandler<CreateDialogCommand>>(); 
            builder.RegisterType<LeaveDialogCommandHandler>().As<ICommandHandler<LeaveDialogCommand>>(); 
            builder.RegisterType<InviteParticipantsCommandHandler>().As<ICommandHandler<InviteParticipantsCommand>>();
            builder.RegisterType<UpdateDialogInfoCommandHandler>().As<ICommandHandler<UpdateDialogInfoCommand>>().WithAttributeFiltering();
            builder.RegisterType<StartUploadContentCommandHandler>().As<ICommandHandler<StartUploadContentCommand>>().WithAttributeFiltering();
        }
    }
}
