using Funkmap.Domain.Models;

namespace Funkmap.Domain.Events
{
    public class ProfileDeletedEvent
    {
        public Profile Profile { get; set; }
    }
}
