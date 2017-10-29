using System;
using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Settings;
using NLog;
using NLog.Config;

namespace Funkmap.Common.Logger
{
    public class LoggerModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.Register(container =>
            {
                var settingsService = container.Resolve<ISettingsService>();
                var settings = settingsService.GetSettings();

                var logConfig = "NLog.config";

                switch (settings.LoggingType)
                {
                        case LoggingType.Empty:
                            logConfig = "";
                        break;

                        case LoggingType.File:
                            
                        break;

                        case LoggingType.Email:
                            logConfig = "NLogEmail.config";
                        break;
                }

                if(!String.IsNullOrEmpty(logConfig)) LogManager.Configuration = new XmlLoggingConfiguration(logConfig);
                var logger = LogManager.GetCurrentClassLogger();
                return logger;
            }).As<ILogger>();

            builder.RegisterGeneric(typeof(FunkmapLogger<>)).As(typeof(IFunkmapLogger<>));
        }
    }
}
