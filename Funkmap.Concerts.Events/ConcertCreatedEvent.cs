using Funkmap.Concerts.Entities;

namespace Funkmap.Concerts.Events
{
    public class ConcertCreatedEvent
    {
        public ConcertCreatedEvent(ConcertEntity concert)
        {
            Concert = concert;
        }
        public ConcertEntity Concert { get; }
    }
}
