
using System.Collections.Generic;

namespace Funkmap.Messenger.Data.Parameters
{
    public class DialogsNewMessagesParameter
    {
        public string Login { get; set; }
        public ICollection<string> DialogIds { get; set; }
    }
}
