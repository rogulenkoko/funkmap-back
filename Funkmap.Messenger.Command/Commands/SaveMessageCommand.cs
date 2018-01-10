using System.Collections.Generic;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Command.Commands
{
    public class SaveMessageCommand
    {
        public string DialogId { get; set; }
       
        public string Sender { get; set; }
        
        public string Text { get; set; }
        
        public List<ContentItem> Content { get; set; }

        public ICollection<string> UsersWithOpenedCurrentDialog { get; set; }
    }
}
