using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Models
{
    public class ChangeAvatarRequest
    {
        public string Login { get; set; }
        public byte[] Avatar { get; set; }
    }
}
