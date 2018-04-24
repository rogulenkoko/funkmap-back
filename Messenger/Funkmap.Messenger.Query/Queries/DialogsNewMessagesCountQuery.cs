using System.Collections.Generic;

namespace Funkmap.Messenger.Query.Queries
{
    public class DialogsNewMessagesCountQuery
    {
        public DialogsNewMessagesCountQuery(string userLogin)
        {
            UserLogin = userLogin;
        }

        public string UserLogin { get; }
    }
}
