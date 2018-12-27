using Funkmap.Common.Notifications.Notification;

namespace Funkmap.Auth.Notifications
{
    /// <summary>
    /// Password recovery notification
    /// </summary>
    public class PasswordRecoverNotification : Notification
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver">Receiver</param>
        /// <param name="name">User's name</param>
        /// <param name="code">Confirmation code</param>
        public PasswordRecoverNotification(string receiver, string name, string code) : base(receiver)
        {
            Subject = GetSubject();
            Title = GetConfirmationTitle(name);
            MainContent = "<br><br><br>";
            Footer = GetFooter(code);
        }

        private string GetSubject()
        {
            return Resources.Resource.PasswordRecovery;
        }

        private string GetConfirmationTitle(string name)
        {
            return $"{Resources.Resource.Registration_Greeting}, {name}";
        }

        private string GetFooter(string code)
        {
            return $"{Resources.Resource.PasswordRecovery_Code}: " + $"<span style=\"font-family: 'Roboto', Arial, sans-serif; margin: 0 0 0; font-size: 26px; line-height: 29px; color:#ee9117;\"> {code} </span>";
        }
    }
}
