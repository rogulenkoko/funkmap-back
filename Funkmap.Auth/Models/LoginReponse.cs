using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Module.Auth.Models
{
    public class LoginReponse
    {
        public bool IsExist { get; set; }
        public string Token { get; set; }
    }
}
