using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Common.Notification
{
    public abstract class Notification
    {
        public string Receiver { get; set; }
        public virtual string Body { get; set; }
        public virtual string Subject { get; set; }
    }
}
