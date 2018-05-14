using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Funkmap.Middleware.Settings
{
    public  static class JwtTokenOptions
    {
        public static string Issuer { get; } = "http://bandmap-api.net";

        public static SecurityKey Key { get; } = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("funkmap"));
    }
}
