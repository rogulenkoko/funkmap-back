using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Data.Parameters
{
    public class ChangeAvatarParameter
    {
        public string Login { get; set; }
        public string UserLogin { get; set; }
        public byte[] Avatar { get; set; }
    }
}
