using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Notifications.Notification;
using Funkmap.Common.Notifications.Notification.Abstract;
using Funkmap.Common.Notifications.Settings;
using Funkmap.Common.Settings;

namespace Funkmap.Common.Notifications
{
    public class NotificationToolModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<NotificationSettingsService>().As<ISettingsService>();
            builder.RegisterType<EmailExternalNotificationService>().As<IExternalNotificationService>();
        }
    }
}
