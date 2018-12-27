using Funkmap.Common.Notifications.Notification;

namespace Funkmap.Auth.Notifications
{
    /// <summary>
    /// Confirmation notification
    /// </summary>
    public class ConfirmationNotification : Notification
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="receiver">Receiver</param>
        /// <param name="name">User's name</param>
        /// <param name="code">Confirmation code</param>
        public ConfirmationNotification(string receiver, string name, string code) : base(receiver)
        {
            Subject = GetSubject();
            Title = GetConfirmationTitle(name);
            MainContent = GetConfirmationMainContent();
            Footer = GetConfirmationFooter(code);
        }


        private string GetSubject()
        {
            return Resources.Resource.Registration_Confirmation;
        }

        private string GetConfirmationTitle(string name)
        {
            return $"{Resources.Resource.Registration_Greeting}, {name}";
        }

        private string GetConfirmationMainContent()
        {
            return $"{Resources.Resource.Registration_Gratitude}. <br> {Resources.Resource.Registration_Appreciate} <br><br> {Resources.Resource.Registration_Welcome}.";
        }

        private string GetConfirmationFooter(string code)
        {
            return $"{Resources.Resource.Registration_ConfirmationCode}: " + $"<span style=\"font-family: 'Roboto', Arial, sans-serif; margin: 0 0 0; font-size: 26px; line-height: 29px; color:#ee9117;\"> {code} </span>";
        }
    }
}
