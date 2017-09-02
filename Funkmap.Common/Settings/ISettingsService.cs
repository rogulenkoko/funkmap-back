
namespace Funkmap.Common.Settings
{
    public interface ISettingsService
    {
        ISettings GetSettings();
    }

    public interface ISettings
    {
        bool EnableLogs { get; }

        string Email { get; }

        string EmailPassword { get; }
    }
}
