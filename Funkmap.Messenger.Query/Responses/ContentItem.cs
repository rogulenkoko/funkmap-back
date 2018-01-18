using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Query.Responses
{
    public class ContentItem
    {
        public ContentType ContentType { get; set; }
        
        public string FileName { get; set; }
        public string FileId { get; set; }
        public byte[] FileBytes { get; set; }

        public double Size { get; set; }
    }
}
