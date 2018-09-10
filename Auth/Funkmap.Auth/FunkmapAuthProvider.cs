using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Funkmap.Auth.Contracts;
using Funkmap.Auth.Domain.Abstract;
using Funkmap.Auth.Domain.Models;
using Funkmap.Auth.Services;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Funkmap.Auth
{
    public class FunkmapAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IAuthRepository _repository;
        private readonly SocialUserFacade _socialUserFacade;

        public FunkmapAuthProvider(IAuthRepository repository, SocialUserFacade socialUserFacade)
        {
            _repository = repository;
            _socialUserFacade = socialUserFacade;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            User user;
            Claim userNameClaim;
            //если не указаны логин и пароль, то значит авторизация через соц.сети
            if (String.IsNullOrEmpty(context.UserName) && string.IsNullOrEmpty(context.Password))
            {
                IFormCollection parameters = await context.Request.ReadFormAsync();
                var provider = parameters.Get("provider");
                var token = parameters.Get("token");
                _socialUserFacade.TryGetSocialUser(token, provider, out user);

                var existingSocialUser = await _repository.GetAsync(user.Login);

                if (existingSocialUser == null)
                {
                    var result = await _repository.TryCreateSocialAsync(user);
                    if (!result.Success)
                    {
                        context.SetError("invalid_grant", "Operation failed");
                        return;
                    }
                }

                userNameClaim = new Claim(ClaimTypes.Name, user.Login);
            }
            else
            {
                var passwordHash = CryptoProvider.ComputeHash(context.Password);
                user = await _repository.LoginAsync(context.UserName, passwordHash);
                userNameClaim = new Claim(ClaimTypes.Name, context.UserName);
            }


            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            identity.AddClaim(userNameClaim);

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
