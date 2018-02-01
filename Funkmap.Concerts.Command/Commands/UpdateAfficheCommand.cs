namespace Funkmap.Concerts.Command.Commands
{
    public class UpdateAfficheCommand
    {
        public string ConcertId { get; set; }

        public string User { get; set; }

        public byte[] Data { get; set; }
    }
}
