using Funkmap.Common.Notifications.Notification;

namespace Funkmap.Module.Auth.Notifications
{
    public class PasswordRecoverNotification : Notification
    {
        public PasswordRecoverNotification(string reciever, string name, string newPassword) : base(reciever)
        {
            Subject = GetSubject();
            Title = GetConfirmationTitle(name);
            MainContent = "<br><br><br>";
            Footer = GetFooter(newPassword);
        }

        private string GetSubject()
        {
            return "Восстановление пароля";
        }

        private string GetConfirmationTitle(string name)
        {
            return $"Приветствую, {name}";
        }

        private string GetFooter(string newPassword)
        {
            return "Ваш пароль: " + $"<span style=\"font-family: 'Roboto', Arial, sans-serif; margin: 0 0 0; font-size: 26px; line-height: 29px; color:#ee9117;\"> {newPassword} </span>";
        }
    }
}
