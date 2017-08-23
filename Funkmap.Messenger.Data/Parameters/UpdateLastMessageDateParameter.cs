using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Messenger.Data.Parameters
{
    public class UpdateLastMessageDateParameter
    {
        public DateTime Date { get; set; }
        public string DialogId { get; set; }
    }
}
