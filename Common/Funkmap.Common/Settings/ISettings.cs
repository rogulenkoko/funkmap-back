using System;
using System.Configuration;

namespace Funkmap.Common.Settings
{
    public interface ISettings
    {
        bool EnableLogs { get; }

        string Email { get; }

        string EmailPassword { get; }

        LoggingType LoggingType { get; }
    }

    public class SettingsBase : ISettings
    {
        public virtual bool EnableLogs
        {
            get
            {
                bool enableLogs;
                Boolean.TryParse(ConfigurationManager.AppSettings["enableLogs"], out enableLogs);
                return enableLogs;
            }
        }

        public virtual string Email
        {
            get
            {
                var appEmail = ConfigurationManager.AppSettings["email"];
                if (String.IsNullOrEmpty(appEmail))
                {
                    throw new InvalidOperationException("Необходимо записать почту приложения в app.config");
                }
                return appEmail;
            }
        }

        public virtual string EmailPassword
        {
            get
            {
                var appEmailPassword = ConfigurationManager.AppSettings["emailPassword"];
                if (String.IsNullOrEmpty(appEmailPassword))
                {
                    throw new InvalidOperationException("Необходимо записать пароль от почты приложения в app.config");
                }
                return appEmailPassword;
            }
        }

        public virtual LoggingType LoggingType
        {
            get
            {
                LoggingType loggingType;
                Enum.TryParse(ConfigurationManager.AppSettings["logging-type"], out loggingType);
                return loggingType;
            }
        }
    }

    public enum LoggingType
    {
        Empty = 0,
        File = 1,
        Email = 2
    }
}
