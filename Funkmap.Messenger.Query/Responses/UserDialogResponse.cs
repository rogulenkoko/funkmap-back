
namespace Funkmap.Messenger.Query.Responses
{
    public class UserDialogResponse
    {
        public UserDialogResponse(bool success, DialogWithLastMessage dialog)
        {
            Success = success;
            Dialog = dialog;
        }

        public bool Success { get; }

        public DialogWithLastMessage Dialog { get; }
    }
}
