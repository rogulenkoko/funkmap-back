using System;

namespace Funkmap.Messenger.Command.Commands
{
    public class ReadMessagesCommand
    {
        public string DialogId { get; set; }

        public DateTime ReadTime { get; set; }

        public string UserLogin { get; set; }
    }
}
