using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Command.Commands
{
    public class StartUploadContentCommand
    {
        public StartUploadContentCommand(ContentType contentType, string fileName, byte[] fileBytes, string sender)
        {
            ContentType = contentType;
            FileName = fileName;
            FileBytes = fileBytes;
            Sender = sender;
        }
        public ContentType ContentType { get; }
        
        public string FileName { get; }
        
        public byte[] FileBytes { get; set; }

        public string Sender { get; }
    }
}
