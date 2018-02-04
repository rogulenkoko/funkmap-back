
namespace Funkmap.Concerts.Command.Commands
{
    public class UpdateActivityCommand
    {
        public string ConcertId { get; set; }

        public bool IsActive { get; set; }
    }
}
