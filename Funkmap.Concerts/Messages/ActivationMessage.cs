
namespace Funkmap.Concerts.Messages
{
    public class ActivationMessage
    {
        public ActivationMessage(string concertId)
        {
            ConcertId = concertId;
        }
        public string ConcertId { get; set; }
    }
}
