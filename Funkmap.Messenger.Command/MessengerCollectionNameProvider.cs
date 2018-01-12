namespace Funkmap.Messenger.Command
{
    public static class MessengerCollectionNameProvider
    { 
        public const string DialogsCollectionName = "dialogs";
        public const string MessagesCollectionName = "messages";
        public static readonly string MessagesBucketCollectionName = $"{MessagesCollectionName}.files";

        public const string MessengerStorage = "messengerstorage";
    }
}
