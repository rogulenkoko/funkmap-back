using Funkmap.Notifications.Contracts;

namespace Funkmap.Domain.Notifications
{
    [FunkmapNotification("priority_marker", false)]
    public class PriorityMarkerNotification
    {
        public string ProfileLogin { get; set; }
    }
}
