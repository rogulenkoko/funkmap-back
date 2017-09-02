
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

        LoggingType LoggingType { get; }


    }

    public enum LoggingType
    {
        Empty = 0,
        File = 1,
        Email = 2
    }
}
