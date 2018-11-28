namespace Funkmap.Notifications.Contracts.Abstract
{
    /// <summary>
    /// Base email notification
    /// </summary>
    public interface IFunkmapEmailNotification
    {
        /// <summary>
        /// Notification subject
        /// </summary>
        string Subject { get; }
        
        /// <summary>
        /// Notification html body
        /// </summary>
        string Body { get; }
    }
}