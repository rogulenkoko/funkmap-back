using System;
using System.Collections.Generic;

namespace Funkmap.Messenger.Command.Commands
{
    public class CreateDialogCommand
    {
        public string CreatorLogin { get; set; }

        public List<string> Participants { get; set; }

        /// <summary>
        /// Set dialog name if participants count is greater then 2
        /// </summary>
        public string DialogName { get; set; }
    }
}
