namespace Funkmap.Messenger
{
    public static class MessengerCollectionNameProvider
    { 
        public static string DialogsCollectionName = "dialogs";
        public static string MessagesCollectionName = "messages";
        public static string MessagesBucketCollectionName = $"{MessagesCollectionName}.files";
    }
}
