using System;
using System.Configuration;
using Funkmap.Common.Settings;

namespace Funkmap.Middleware.Settings
{
    public class MonolithSettings : ISettings
    {
        public bool EnableLogs
        {
            get
            {
                bool enableLogs;
                Boolean.TryParse(ConfigurationManager.AppSettings["enableLogs"], out enableLogs);
                return enableLogs;
            }
        }

        public string Email
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

        public string EmailPassword
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

        public LoggingType LoggingType
        {
            get
            {
                LoggingType loggingType;
                Enum.TryParse(ConfigurationManager.AppSettings["loggingType"], out loggingType);
                return loggingType;
            }
        }
    }
}
