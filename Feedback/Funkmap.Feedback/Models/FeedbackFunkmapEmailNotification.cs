using System.Text;
using Funkmap.Feedback.Domain;
using Funkmap.Notifications.Contracts.Abstract;

namespace Funkmap.Feedback.Models
{
    /// <summary>
    /// Administrator feedback email notification
    /// </summary>
    public class FeedbackEmailNotification : IFunkmapEmailNotification
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FeedbackEmailNotification(FeedbackItem item)
        {
            _item = item;
        }
        private readonly FeedbackItem _item;

        /// <inheritdoc cref="IFunkmapEmailNotification.Subject"/>
        public string Subject
        {
            get
            {
                var sb = new StringBuilder("Обратная связь.");
                switch (_item.FeedbackType)
                {
                    case FeedbackType.Bug:
                        sb.Append(" Баг");
                        break;
                    case FeedbackType.Feature:
                        sb.Append(" Фича");
                        break;
                }

                return sb.ToString();
            }
        }

        /// <inheritdoc cref="IFunkmapEmailNotification.Body"/>
        public string Body
        {
            get
            {
                var sb = new StringBuilder(_item.Message);

                if (_item.Content == null || _item.Content.Count <= 0) return sb.ToString();
                sb.AppendLine();
                sb.AppendLine("Приложенные файлы:");
                foreach (var contentItem in _item.Content)
                {
                    sb.AppendLine(contentItem.DataUrl);
                }
                return sb.ToString();
            }
        }
    }
}