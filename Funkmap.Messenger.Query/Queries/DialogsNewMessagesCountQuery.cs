using System.Collections.Generic;

namespace Funkmap.Messenger.Query.Queries
{
    public class DialogsNewMessagesCountQuery
    {
        public DialogsNewMessagesCountQuery(string userLogin, ICollection<string> dialogIds)
        {
            UserLogin = userLogin;
            DialogIds = dialogIds;
        }

        public string UserLogin { get; }
        public ICollection<string> DialogIds { get; }
    }
}
