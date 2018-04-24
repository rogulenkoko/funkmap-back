namespace Funkmap.Messenger.Query.Responses
{
    public class DialogAvatarInfoResponse
    {
        public DialogAvatarInfoResponse(bool success, byte[] avatarInfo, string dialogId)
        {
            Success = success;
            AvatarBytes = avatarInfo;
            DialogId = dialogId;
        }

        public bool Success { get; }

        public byte[] AvatarBytes { get; }

        public string DialogId { get; }
    }
}
