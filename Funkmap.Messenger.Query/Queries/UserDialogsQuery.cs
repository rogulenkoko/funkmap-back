
namespace Funkmap.Messenger.Query.Queries
{
    public class UserDialogsQuery
    {
        public UserDialogsQuery(string userLogin)
        {
            UserLogin = userLogin;
        }
        public string UserLogin { get; }
    }
}
