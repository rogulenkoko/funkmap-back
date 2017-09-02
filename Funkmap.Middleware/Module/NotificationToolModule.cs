﻿using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Notifications.Notification;
using Funkmap.Common.Notifications.Notification.Abstract;

namespace Funkmap.Middleware.Module
{
    public class NotificationToolModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<EmailNotificationService>().As<INotificationService>();
        }
    }
}