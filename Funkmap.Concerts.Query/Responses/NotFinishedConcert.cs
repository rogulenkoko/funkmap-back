using System;
using System.Collections.Generic;

namespace Funkmap.Concerts.Query.Responses
{

    public class NotFinishedConcertResponse
    {
        public NotFinishedConcertResponse(ICollection<NotFinishedConcert> concerts)
        {
            Concerts = concerts;
        }

        public ICollection<NotFinishedConcert> Concerts { get; }
    }

    public class NotFinishedConcert
    {
        public string ConcertId { get; set; }

        public DateTime PeriodBeginUtc { get; set; }

        public DateTime PeriodEndUtc { get; set; }
    }
}
