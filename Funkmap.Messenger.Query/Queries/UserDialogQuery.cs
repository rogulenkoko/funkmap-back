
namespace Funkmap.Messenger.Query.Queries
{
    public class UserDialogQuery
    {
        public UserDialogQuery(string dialogId, string userLogin)
        {
            DialogId = dialogId;
            UserLogin = userLogin;
        }

        public string DialogId { get; }
        public string UserLogin { get; }
    }
}
