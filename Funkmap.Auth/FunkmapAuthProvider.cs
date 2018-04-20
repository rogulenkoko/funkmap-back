using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Funkmap.Auth.Domain.Abstract;
using Funkmap.Auth.Domain.Models;
using Funkmap.Auth.Services;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Funkmap.Auth
{
    public class FunkmapAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IAuthRepository _repository;

        public FunkmapAuthProvider(IAuthRepository repository)
        {
            _repository = repository;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var password = CryptoProvider.ComputeHash(context.Password);

            User user = await _repository.LoginAsync(context.UserName, password);
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

            var propertiesDictionary = new Dictionary<string, string>();

            propertiesDictionary[nameof(user.Login)] = user.Login;

            var props = new AuthenticationProperties(propertiesDictionary);

            var ticket = new AuthenticationTicket(identity, props);
            ticket.Properties.AllowRefresh = true;
            context.Validated(ticket);

        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            return base.GrantRefreshToken(context);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}
