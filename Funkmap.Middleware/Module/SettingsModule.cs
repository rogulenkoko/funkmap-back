using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Settings;
using Funkmap.Middleware.Settings;

namespace Funkmap.Middleware.Module
{
    public class SettingsModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MonolithSettingsService>().As<ISettingsService>();
        }
    }
}
