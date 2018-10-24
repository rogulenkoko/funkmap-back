using Funkmap.Notifications.Contracts;

namespace Funkmap.Domain.Notifications
{
    [FunkmapNotification("pro_account", false)]
    public class ProAccountNotification
    {
        public string Login { get; set; }
    }
}
