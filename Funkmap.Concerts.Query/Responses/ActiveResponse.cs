using System.Collections.Generic;

namespace Funkmap.Concerts.Query.Responses
{
    public class ActiveResponse
    {
        public ICollection<ActiveConcertMarker> ConcertMarkers { get; set; }
    }

    public class ActiveConcertMarker
    {
        public string Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
