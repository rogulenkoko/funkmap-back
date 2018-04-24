namespace Funkmap.Messenger.Query.Queries
{
    public class DialogMessagesQuery
    {
        public string DialogId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string UserLogin { get; set; }
    }
}
