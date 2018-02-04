using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace Funkmap.Concerts.Models
{
    public class CreateConcertRequest
    {
        [Required]
        public DateTime PeriodBegin { get; set; }

        [Required]
        public DateTime PeriodEnd { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] AfficheBytes { get; set; }

        /// <summary>
        /// Логины профилей, которые принимают участие к концерту
        /// </summary>
        public List<string> Participants { get; set; }
    }
}
