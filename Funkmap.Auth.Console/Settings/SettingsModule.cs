using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Settings;

namespace Funkmap.Auth.Console.Settings
{
    public class SettingsModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<AuthSettingsService>().As<ISettingsService>();
        }
    }
}
