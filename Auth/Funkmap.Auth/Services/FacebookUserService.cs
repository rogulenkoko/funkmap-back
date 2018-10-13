using System;
using System.Net.Http;
using Funkmap.Auth.Contracts;
using Newtonsoft.Json;

namespace Funkmap.Auth.Services
{
    public class FacebookUserService : ISocialUserService
    {
        public string Provider => "facebook";

        private readonly HttpClient _http;

        private const string _facebookUrl = "https://graph.facebook.com";

        public FacebookUserService(HttpClient http)
        {
            _http = http;
        }

        public bool TryGetUser(string token, out User user)
        {
            try
            {
                var userInfoJson = _http.GetStringAsync($"{_facebookUrl}/me?fields=id,name,email,picture&access_token={token}").GetAwaiter().GetResult();

                if (String.IsNullOrEmpty(userInfoJson))
                {
                    user = null;
                    return false;
                }

                FacebookUser facebookUser = JsonConvert.DeserializeObject<FacebookUser>(userInfoJson);

                user = new User()
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
            catch (Exception e)
            {
                user = null;
                return false;
            }
            
        }

        public class FacebookUser
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("picture")]
            public FacebookPicture FacebookPicture { get; set; }
        }

        public class FacebookPicture
        {
            [JsonProperty("data")]
            public FacebookPictureData Data { get; set; }
        }

        public class FacebookPictureData
        {
            [JsonProperty("url")]
            public string AvatarUrl { get; set; }
        }
    }
}