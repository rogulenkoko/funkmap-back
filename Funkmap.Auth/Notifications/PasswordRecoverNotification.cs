using Funkmap.Common.Notifications.Notification;

namespace Funkmap.Auth.Notifications
{
    public class PasswordRecoverNotification : Notification
    {
        public PasswordRecoverNotification(string reciever, string name, string code) : base(reciever)
        {
            Subject = GetSubject();
            Title = GetConfirmationTitle(name);
            MainContent = "<br><br><br>";
            Footer = GetFooter(code);
        }

        private string GetSubject()
        {
            return "Восстановление пароля";
        }

        private string GetConfirmationTitle(string name)
        {
            return $"Приветствую, {name}";
        }

        private string GetFooter(string code)
        {
            return "Код для восстановления пароля: " + $"<span style=\"font-family: 'Roboto', Arial, sans-serif; margin: 0 0 0; font-size: 26px; line-height: 29px; color:#ee9117;\"> {code} </span>";
        }
    }
}
