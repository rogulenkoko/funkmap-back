using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace Funkmap.Messenger.Models.Requests
{
    public class DialogsRequest
    {
        [Required]
        public int Skip { get; set; }
        [Required]
        public int Take { get; set; }
    }
}
