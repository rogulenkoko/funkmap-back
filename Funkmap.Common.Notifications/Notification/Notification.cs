using System;

namespace Funkmap.Common.Notifications.Notification
{
    public abstract class Notification
    {
        protected Notification(string receiver)
        {
            Receiver = receiver;
            Title = String.Empty;
            Subject = String.Empty;
            MainContent = String.Empty;
            Footer = String.Empty;
        }

        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string MainContent { get; set; }
        public string Footer { get; set; }

    }
}
