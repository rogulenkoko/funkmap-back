using Funkmap.Feedback.Entities;
using Microsoft.Build.Framework;

namespace Funkmap.Feedback.Models
{
    public class FeedbackItem
    {
        [Required]
        public FeedbackType FeedbackType { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
