using System;
using System.Net.Http;
using Funkmap.Auth.Contracts;
using Newtonsoft.Json;

namespace Funkmap.Auth.Services
{
    /// <summary>
    /// Google authorization service
    /// </summary>
    public class GoogleUserService : ISocialUserService
    {
        /// <inheritdoc cref="ISocialUserService.Provider"/>
        public string Provider => "google";

        private readonly HttpClient _http;

        private const string GoogleUrl = "https://www.googleapis.com";

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="http"><see cref="HttpClient"/></param>
        public GoogleUserService(HttpClient http)
        {
            _http = http;
        }

        /// <inheritdoc cref="ISocialUserService.TryGetUser"/>
        public bool TryGetUser(string token, out User user)
        {
            try
            {
                var userInfoJson = _http.GetStringAsync($"{GoogleUrl}/oauth2/v3/tokeninfo?id_token={token}").GetAwaiter().GetResult();

                if (String.IsNullOrEmpty(userInfoJson))
                {
                    user = null;
                    return false;
                }

                var googleUser = JsonConvert.DeserializeObject<GoogleUser>(userInfoJson);

                user = new User
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

        /// <summary>
        /// Google user model
        /// </summary>
        public class GoogleUser
        {
            /// <summary>
            /// Email
            /// </summary>
            [JsonProperty("email")]
            public string Email { get; set; }

            /// <summary>
            /// Name
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// Avatar url
            /// </summary>
            [JsonProperty("picture")]
            public string AvatarUrl { get; set; }
        }
    }
}
