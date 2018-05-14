using System;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Thinktecture.IdentityModel.Tokens;

namespace Funkmap.Middleware.Settings
{
    public class FunkmapJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string audienceId = FunkmapJwtOptions.Audience;

            HmacSigningCredentials signingKey = new HmacSigningCredentials(FunkmapJwtOptions.Key);

            var issued = data.Properties.IssuedUtc;

            var expires = data.Properties.ExpiresUtc;

            var token = new JwtSecurityToken(FunkmapJwtOptions.Issuer, audienceId, data.Identity.Claims,
                issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.WriteToken(token);

            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}