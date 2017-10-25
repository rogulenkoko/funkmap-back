namespace Funkmap.Common.Notifications.Notification
{
    public abstract class Notification
    {
        protected Notification(string receiver)
        {
            Receiver = receiver;
        }

        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string MainContent { get; set; }
        public string Footer { get; set; }

    }
}
