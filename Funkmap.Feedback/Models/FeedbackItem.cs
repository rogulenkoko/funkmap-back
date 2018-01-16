namespace Funkmap.Feedback.Models
{
    public class FeedbackItem
    {
        public FeedbackType FeedbackType { get; set; }

        public string Message { get; set; }
    }

    //todo вынести это в сборку Funkmap.Feedback.Entities
    public enum FeedbackType
    {
        Another = 0,
        Bug = 1,
        Feature = 2
    }
}
