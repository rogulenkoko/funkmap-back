using Funkmap.Common.Settings;

namespace Funkmap.Console.Settings
{
    public class FunkmapSettingsService : ISettingsService
    {
        public ISettings GetSettings()
        {
            return new FunkmapSettings();
        }
    }
}
