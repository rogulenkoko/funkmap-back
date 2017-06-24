using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using MongoDB.Driver;

namespace Funkmap.Module.Auth
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
            
            UserEntity user = await _repository.Login(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

            var propertiesDictionary = new Dictionary<string,string>();

            propertiesDictionary[nameof(user.Login)] = user.Login;

            var props = new AuthenticationProperties(propertiesDictionary);

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

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
