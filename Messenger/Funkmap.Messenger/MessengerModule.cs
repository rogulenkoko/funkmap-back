using System;
using Autofac;
using Funkmap.Cqrs.Abstract;
using Funkmap.Messenger.Command.EventHandlers;
using Funkmap.Messenger.Contracts.Events;
using Funkmap.Messenger.Contracts.Events.Dialogs;
using Funkmap.Messenger.Contracts.Events.Messages;
using Funkmap.Messenger.Handlers;
using Funkmap.Messenger.Services;
using Funkmap.Messenger.Services.Abstract;

namespace Funkmap.Messenger
{
    public static class MessengerModule
    {
        public static void RegisterMessengerDomainModule(this ContainerBuilder builder)
        {
            builder.RegisterType<MessengerConnectionService>().As<IMessengerConnectionService>().SingleInstance();

            builder.RegisterType<DialogLastMessageEventHandler>()
                .As<IEventHandler<MessageSavedCompleteEvent>>()
                .As<IEventHandler>()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            builder.RegisterType<DialogCreatedEventHandler>()
                .As<IEventHandler<DialogCreatedEvent>>()
                .As<IEventHandler>()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            builder.RegisterType<SignalrEventHandler>()
                .As<IEventHandler<DialogUpdatedEvent>>()
                .As<IEventHandler<MessageSavedCompleteEvent>>()
                .As<IEventHandler<MessagesReadEvent>>()
                .As<IEventHandler<DialogCreatedEvent>>()
                .As<IEventHandler<MessengerCommandFailedEvent>>()
                .As<IEventHandler>()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            builder.RegisterType<CommandFailedEventHandler>()
                .As<IEventHandler<MessengerCommandFailedEvent>>()
                .As<IEventHandler>()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            builder.RegisterType<UserLeavedDialogEventHandler>()
                .As<IEventHandler<UserLeavedDialogEvent>>()
                .As<IEventHandler>()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            builder.RegisterType<UserInvitedToDialogEventHandler>()
                .As<IEventHandler<UserInvitedToDialogEvent>>()
                .As<IEventHandler>()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            Console.WriteLine("Mesenger module has been loaded.");
        }
    }
}
