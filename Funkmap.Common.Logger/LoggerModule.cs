using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Funkmap.Common.Abstract;
using NLog;
using NLog.Config;

namespace Funkmap.Common.Logger
{
    public class LoggerModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            LogManager.Configuration = new XmlLoggingConfiguration("NLog.config");
            var logger = LogManager.GetCurrentClassLogger();
            builder.RegisterInstance(logger).As<ILogger>();
            builder.RegisterGeneric(typeof(FunkmapLogger<>)).As(typeof(IFunkmapLogger<>));
        }
    }
}
