using System;
using Autofac;
using Funkmap.Cqrs.Abstract;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Abstract;
using Funkmap.Notifications.Domain.Services;
using Funkmap.Notifications.Domain.Services.Abstract;
using Funkmap.Notifications.SignalR;

namespace Funkmap.Notifications
{
    public static class NotificationsModule
    {
        public static void RegisterNotificationModule(this ContainerBuilder builder)
        {
            builder.RegisterType<NotificationsConnectionService>().As<INotificationsConnectionService>().SingleInstance();

            builder.RegisterType<NotificationService>()
                .As<IEventHandler>()
                .SingleInstance()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            builder.RegisterType<FunkmapNotificationService>().As<IFunkmapNotificationService>();
            
            builder.RegisterType<NotificationsSignalRHandler>()
                .As<IEventHandler>()
                .SingleInstance()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            Console.WriteLine("Notifications module has been loaded.");
        }
    }
}
