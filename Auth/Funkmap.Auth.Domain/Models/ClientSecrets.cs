namespace Funkmap.Auth.Domain.Models
{
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
