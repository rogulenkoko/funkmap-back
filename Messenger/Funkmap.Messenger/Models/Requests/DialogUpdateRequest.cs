
namespace Funkmap.Messenger.Models.Requests
{
    public class DialogUpdateRequest
    {
        public string DialogId { get; set; }
        public string Name { get; set; }

        public byte[] Avatar { get; set; }
    }
}
