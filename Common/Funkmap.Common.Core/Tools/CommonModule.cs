using System;
using System.IO;
using System.Reflection;
using Autofac;
using Funkmap.Common.Settings;
using Funkmap.Cqrs;
using Funkmap.Cqrs.Abstract;
using Funkmap.Logger;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;

namespace Funkmap.Common.Core.Tools
{
    public static class CommonModule
    {
        public static void RegisterCommonModule(this ContainerBuilder builder, IConfiguration config)
        {
            builder.RegisterType<InMemoryEventBus>().As<IEventBus>();
            builder.RegisterType<InMemoryCommandBus>().As<ICommandBus>();
            builder.RegisterType<CommandHandlerResolver>().As<ICommandHandlerResolver>();
            builder.RegisterType<QueryContext>().As<IQueryContext>();
            
            builder.Register(container =>
            {
                var localPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
                var email = Path.Combine(localPath, "NLogEmail.config");
                var file = Path.Combine(localPath, "NLog.config");

                var loggingType = (LoggingType)Enum.Parse(typeof(LoggingType), config["Logging:Type"]);
                
                string fileName;
                switch (loggingType)
                {
                    case LoggingType.Empty:
                        fileName = "";
                        break;
                    case LoggingType.File:
                        fileName = file;
                        break;
                    case LoggingType.Email:
                        fileName = email;
                        break;
                    default:
                        fileName = file;
                        break;
                }
                
                if (!string.IsNullOrEmpty(fileName))
                    LogManager.Configuration = (LoggingConfiguration) new XmlLoggingConfiguration(fileName);
                return LogManager.GetCurrentClassLogger();
            }).SingleInstance().As<ILogger>();
            
            builder.RegisterGeneric(typeof (FunkmapLogger<>)).As(new Type[1]
            {
                typeof (IFunkmapLogger<>)
            });
        }
    }
}