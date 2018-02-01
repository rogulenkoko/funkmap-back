using System;
using System.Collections.Generic;

namespace Funkmap.Concerts.Command.Commands
{
    public class CreateConcertCommand
    {
        
        public DateTime PeriodBegin { get; set; }
        
        public DateTime PeriodEnd { get; set; }
        
        public DateTime Date { get; set; }
        
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
