using Funkmap.Common.Settings;

namespace Funkmap.Auth.Console.Settings
{
    public class AuthSettingsService : ISettingsService
    {
        public ISettings GetSettings()
        {
            return new AuthSettings();
        }
    }
}
