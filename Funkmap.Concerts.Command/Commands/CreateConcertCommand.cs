using System;
using System.Collections.Generic;

namespace Funkmap.Concerts.Command.Commands
{
    public class CreateConcertCommand
    {
        
        public DateTime PeriodBeginUtc { get; set; }
        
        public DateTime PeriodEndUtc { get; set; }
        
        public DateTime DateUtc { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public byte[] AfficheBytes { get; set; }

        public string CreatorLogin { get; set; }

        /// <summary>
        /// Логины профилей, которые принимают участие к концерту
        /// </summary>
        public List<string> Participants { get; set; }
    }
}
