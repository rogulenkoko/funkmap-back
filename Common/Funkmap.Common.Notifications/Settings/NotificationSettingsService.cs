using System.Configuration;
using Funkmap.Common.Settings;

namespace Funkmap.Common.Notifications.Settings
{
    public class NotificationSettingsService : ISettingsService
    {
        public ISettings GetSettings() => new NotificationSettings();
    }

    public class NotificationSettings : ISettings
    {
        public bool EnableLogs => false;
        public string Email => ConfigurationManager.AppSettings["email"];
        public string EmailPassword => ConfigurationManager.AppSettings["emailPassword"];
        public LoggingType LoggingType => LoggingType.Empty;
    }
}
