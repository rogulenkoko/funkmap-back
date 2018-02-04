
namespace Funkmap.Concerts.Messages
{
    public class FinishConcertMessage
    {
        public FinishConcertMessage(string concertId)
        {
            ConcertId = concertId;
        }
        public string ConcertId { get; }
    }
}
