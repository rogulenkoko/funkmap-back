using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Notifications.Contracts.Specific;

namespace Funkmap.Models.Notifications
{
    public class BandInviteNotificationAnswer
    {
        public bool Answer { get; set; }

        public BandInviteNotification Notification { get; set; }
    }
}
