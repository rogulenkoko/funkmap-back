using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Funkmap.Module.Auth.Abstract;
using Funkmap.Module.Auth.Services;
using Funkmap.Module.Auth.Services.External;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Funkmap.Module.Auth
{
    public class FunkmapAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IAuthRepository _repository;
        private readonly IClientSecretProvider _clientSecretProvider;
        private readonly ExternalAuthFacade _externalAuthFacade;
        private readonly IRegistrationContextManager _registrationContextManager;

        public FunkmapAuthProvider(IAuthRepository repository, 
                                   IClientSecretProvider clientSecretProvider, 
                                   IRegistrationContextManager registrationContextManager, 
                                   ExternalAuthFacade externalAuthFacade)
        {
            _repository = repository;
            _clientSecretProvider = clientSecretProvider;
            _externalAuthFacade = externalAuthFacade;
            _registrationContextManager = registrationContextManager;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;

            if (!context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
                {
                    context.SetError("Invalid application credantionals");
                    return Task.FromResult<object>(null);
                }
            }

            if (!_clientSecretProvider.ValidateClient(clientId, clientSecret))
            {
                context.SetError("Invalid application credantionals");
                return Task.FromResult<object>(null);
            }

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantCustomExtension(OAuthGrantCustomExtensionContext context)
        {
            if (context.GrantType != "external")
            {
                context.SetError("Invalid grant type");
                return;
            }
            
            var token = context.Parameters.Get("token");

            AuthProviderType type;
            var parseResult = Enum.TryParse(context.Parameters.Get("provider"), out type);

            if (String.IsNullOrEmpty(token) || !parseResult)
            {
                context.SetError("Invalid parameters");
                return;
            }

            var user = await _externalAuthFacade.BuildUserAsync(token, type);

            if (user == null)
            {
                context.SetError("Invalid user");
                return;
            }

            var isExist = await _repository.CheckIfExistAsync(user.Login);
            
            if (!isExist)
            {
                var validateResult = await _registrationContextManager.ValidateLogin(user.Login);

                if (!validateResult)
                {
                    context.SetError("External validation error");
                    return;
                }

                var registerResult = await _registrationContextManager.TryRegisterExternal(user, type);
                if (!registerResult)
                {
                    context.SetError("External validation error");
                    return;
                }
            }

            context.Validated(BuildTicket(context, user.Login));
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var password = CryptoProvider.ComputeHash(context.Password);
            
            UserEntity user = await _repository.LoginAsync(context.UserName, password);
            if (user == null)
            {
                context.SetError("Invalid grant type", "The user name or password is incorrect.");
                return;
            }

            context.Validated(BuildTicket(context, user.Login));
        }

        private AuthenticationTicket BuildTicket(BaseValidatingTicketContext<OAuthAuthorizationServerOptions> context, string login)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, login));

            var propertiesDictionary = new Dictionary<string, string>();

            propertiesDictionary["Login"] = login;

            var props = new AuthenticationProperties(propertiesDictionary);

            var ticket = new AuthenticationTicket(identity, props);
            ticket.Properties.AllowRefresh = true;
            return ticket;
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
