using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;

namespace Funkmap.Middleware.Settings
{
    public class AccessTokenFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly JwtSecurityTokenHandler _handler;

        public AccessTokenFormat()
        {
            _handler = new JwtSecurityTokenHandler();
        }

        public string Protect(AuthenticationTicket data)
        {
            var issued = data.Properties.IssuedUtc?.UtcDateTime;
            var expires = data.Properties.ExpiresUtc?.UtcDateTime;
            var issuer = "funkmap";


            string symmetricKeyAsBase64 = audience.Base64Secret;

            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            var signingKey = new HmacSigningCredentials(keyByteArray);

            SecurityKey signingKey = new RsaSecurityKey;

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Issuer = issuer,
                Audience = issuer,
                Expires = expires,
                IssuedAt = issued,
                SigningCredentials = new SigningCredentials(signingKey, "RSA")
            };

            var token = _handler.CreateToken(descriptor);

            return _handler.WriteToken(token);
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}
