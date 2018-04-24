using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Messenger.Models.Requests
{
    public class CreateDialogRequest
    {
        public List<string> Participants { get; set; }
        public string Name { get; set; }
    }
}
