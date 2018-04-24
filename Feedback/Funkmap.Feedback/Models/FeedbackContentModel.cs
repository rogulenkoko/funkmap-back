using Microsoft.Build.Framework;

namespace Funkmap.Feedback.Models
{
    public class FeedbackContentModel
    {

        [Required]
        public string Name { get; set; }

        public double Size { get; set; }

        [Required]
        public byte[] Data { get; set; }
    }
}
