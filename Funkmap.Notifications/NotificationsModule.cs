using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Common.RedisMq;
using Funkmap.Contracts.Notifications;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Services;
using Funkmap.Notifications.Services.Abstract;

namespace Funkmap.Notifications
{
    public class NotificationsModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("Funkmap"))
                .SelectMany(s => s.GetTypes())
                .Where(x => x.GetInterfaces().Contains(typeof(INotificationTypesPair)))
                .Distinct()
                .ToList();

            var instances = types.Select(x => Activator.CreateInstance(x) as INotificationTypesPair).ToList();
            foreach (var instance in instances)
            {
                var serviceType = typeof(NotificationsService<,>).MakeGenericType(new[] { instance.RequestType, instance.ResponseType});
                builder.RegisterType(serviceType).As<INotificationsService>().As<IRedisMqConsumer>();
            }

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль уведомлений");
        }

        public class NotificationServiceFactory
        {
            public ICollection<INotificationsService> NotificationsServices { get; private set; }

            /// <summary>
            /// Инициализирует только в первый раз (NotificationsServices были null)
            /// </summary>
            public void SetNotificationServices(ICollection<INotificationsService> notificationsServices)
            {
                if (NotificationsServices == null)
                {
                    NotificationsServices = notificationsServices;
                }
            }
        }
    }
}
