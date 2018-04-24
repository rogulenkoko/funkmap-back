using Funkmap.Domain.Models;

namespace Funkmap.Domain.Events
{
    public class ProfileUpdatedEvent
    {
        public Profile Profile { get; set; }
        public Profile UpdatedProfile { get; set; }
    }
}
