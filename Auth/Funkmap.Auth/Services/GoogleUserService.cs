using System;
using System.Net.Http;
using Funkmap.Auth.Contracts;
using Funkmap.Auth.Domain.Models;
using Newtonsoft.Json;

namespace Funkmap.Auth.Services
{
    public class GoogleUserService : ISocialUserService
    {
        public string Provider => "google";

        private readonly HttpClient _http;

        private const string _googleUrl = "https://www.googleapis.com";

        public GoogleUserService(HttpClient http)
        {
            _http = http;
        }

        public bool TryGetUser(string token, out User user)
        {
            try
            {
                var userInfoJson = _http.GetStringAsync($"{_googleUrl}/oauth2/v3/tokeninfo?id_token={token}").GetAwaiter().GetResult();

                if (String.IsNullOrEmpty(userInfoJson))
                {
                    user = null;
                    return false;
                }

                GoogleUser googleUser = JsonConvert.DeserializeObject<GoogleUser>(userInfoJson);

                user = new User()
                {
                    Email = googleUser.Email,
                    Name = googleUser.Name,
                    AvatarUrl = googleUser.AvatarUrl,
                    LastVisitDateUtc = DateTime.UtcNow,
                    IsSocial = true,
                    Login = Guid.NewGuid().ToString() //todo
                };

                return true;
            }
            catch (Exception e)
            {
                user = null;
                return false;
            }
        }

        public class GoogleUser
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("picture")]
            public string AvatarUrl { get; set; }
        }
    }
}
