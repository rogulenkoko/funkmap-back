namespace Funkmap.Feedback.Domain
{
    /// <summary>
    /// File content of feedback
    /// </summary>
    public class FeedbackContentModel
    {
        /// <summary>
        /// File name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Size of file
        /// </summary>
        public double Size { get; set; }

        /// <summary>
        /// File data
        /// </summary>
        public byte[] Data { get; set; }
        
        /// <summary>
        /// Data url
        /// </summary>
        public string DataUrl { get; set; }
    }
}