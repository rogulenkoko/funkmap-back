using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
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



                var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName);
                var emailConfig = Path.Combine(root, "NLogEmail.config");
                var fileConfig = Path.Combine(root, "NLog.config");

                string logConfig;

                switch (settings.LoggingType)
                {
                    case LoggingType.Empty:
                        logConfig = "";
                        break;

                    case LoggingType.File:
                        logConfig = fileConfig;
                        break;

                    case LoggingType.Email:
                        logConfig = emailConfig;
                        break;
                    default:
                        logConfig = fileConfig;
                        break;
                }

                if (!String.IsNullOrEmpty(logConfig)) LogManager.Configuration = new XmlLoggingConfiguration(logConfig);
                var logger = LogManager.GetCurrentClassLogger();
                return logger;
            }).SingleInstance().As<ILogger>();

            builder.RegisterGeneric(typeof(FunkmapLogger<>)).As(typeof(IFunkmapLogger<>));
        }
    }
}
