using System;
using System.Net.Http;
using Funkmap.Auth.Contracts;
using Newtonsoft.Json;

namespace Funkmap.Auth.Services
{
    public class FacebookUserService : ISocialUserService
    {
        /// <inheritdoc cref="ISocialUserService.Provider"/>
        public string Provider => "facebook";

        private readonly HttpClient _http;

        private const string FacebookUrl = "https://graph.facebook.com";

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="http"><see cref="HttpClient"/></param>
        public FacebookUserService(HttpClient http)
        {
            _http = http;
        }

        /// <inheritdoc cref="ISocialUserService.TryGetUser"/>
        public bool TryGetUser(string token, out User user)
        {
            try
            {
                var userInfoJson = _http.GetStringAsync($"{FacebookUrl}/me?fields=id,name,email,picture&access_token={token}").GetAwaiter().GetResult();

                if (string.IsNullOrEmpty(userInfoJson))
                {
                    user = null;
                    return false;
                }

                var facebookUser = JsonConvert.DeserializeObject<FacebookUser>(userInfoJson);

                user = new User
                {
                    Email = facebookUser.Email,
                    Name = facebookUser.Name,
                    AvatarUrl = facebookUser?.FacebookPicture?.Data?.AvatarUrl,
                    LastVisitDateUtc = DateTime.UtcNow,
                    IsSocial = true,
                    Login = Guid.NewGuid().ToString() //todo
                };

                return true;
            }
            catch (Exception)
            {
                user = null;
                return false;
            }
            
        }

        /// <summary>
        /// Facebook user model
        /// </summary>
        public class FacebookUser
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

            /// <see cref="FacebookPicture"/>
            [JsonProperty("picture")]
            public FacebookPicture FacebookPicture { get; set; }
        }

        /// <summary>
        /// Facebook picture model
        /// </summary>
        public class FacebookPicture
        {
            /// <see cref="FacebookPictureData"/>
            [JsonProperty("data")]
            public FacebookPictureData Data { get; set; }
        }

        /// <summary>
        /// Facebook picture data model
        /// </summary>
        public class FacebookPictureData
        {
            /// <summary>
            /// Facebook avatar url
            /// </summary>
            [JsonProperty("url")]
            public string AvatarUrl { get; set; }
        }
    }
}