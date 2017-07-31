namespace Funkmap.Messenger.Models.Requests
{
    public class DialogMessagesRequest
    {
        public string Reciever { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
