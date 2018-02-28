using System.Collections.Generic;
using Funkmap.Module.Auth.Abstract;

namespace Funkmap.Module.Auth.Services
{
    public class ClientSecretProvider : IClientSecretProvider
    {
        private readonly HashSet<ClientSecrets> _clients;

        public ClientSecretProvider()
        {
            //todo в будущем, доставать из базы креды зарегестрированных клиентских приожений
            _clients = new HashSet<ClientSecrets>() { new ClientSecrets("funkmap", "funkmap") };
        }

        public bool ValidateClient(string clientId, string clientSecret)
        {
            return _clients.Contains(new ClientSecrets(clientId, clientSecret));
        }
    }

    public class ClientSecrets
    {
        public ClientSecrets(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
        public string ClientId { get; }
        public string ClientSecret { get; }

        public override bool Equals(object obj)
        {
            var clientSecret = obj as ClientSecrets;
            if (clientSecret == null) return false;

            return clientSecret.ClientId.Equals(ClientId) && clientSecret.ClientSecret.Equals(ClientSecret);
        }

        public override int GetHashCode()
        {
            return ClientId.GetHashCode() ^ ClientSecret.GetHashCode();
        }
    }
}
