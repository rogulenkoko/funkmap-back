using Funkmap.Common.Notifications.Notification;

namespace Funkmap.Module.Auth.Confirmation
{
    public class ConfirmationNotification : Notification
    {
        public void BuildMessageText(string login, string code)
        {
            Subject = "Confirmation";
            Body = $"Code:{code}";
        }

        public void BuildMessageText(string password)
        {
            Subject = "Confirmation";
            Body = $"You password:{password}";
        }
    }
}
