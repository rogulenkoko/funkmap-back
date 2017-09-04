using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Settings;

namespace Funkmap.Middleware.Settings
{
    public class SettingsModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MonolithSettingsService>().As<ISettingsService>();
        }
    }
}
