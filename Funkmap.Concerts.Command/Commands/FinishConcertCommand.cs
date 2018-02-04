
namespace Funkmap.Concerts.Command.Commands
{
    public class FinishConcertCommand
    {
        public FinishConcertCommand(string concertId)
        {
            ConcertId = concertId;
        }
        public string ConcertId { get; }
    }
}
