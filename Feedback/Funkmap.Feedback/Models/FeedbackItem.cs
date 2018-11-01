using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Funkmap.Feedback.Entities;

namespace Funkmap.Feedback.Models
{
    /// <summary>
    /// Feedback request model
    /// </summary>
    public class FeedbackItem
    {
        /// <summary>
        /// Type of feedback (bug, feature or another)
        /// </summary>
        [Required]
        public FeedbackType FeedbackType { get; set; }

        /// <summary>
        /// Feedback message
        /// </summary>
        [Required]
        public string Message { get; set; }

        /// <summary>
        /// Collection of attached files
        /// </summary>
        public ICollection<FeedbackContentModel> Content { get; set; }
    }
}
