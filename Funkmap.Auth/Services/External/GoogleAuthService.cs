using System;
using System.Net.Http;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Entities;
using Funkmap.Module.Auth.Services.External.Abstract;
using Funkmap.Module.Auth.Services.External.Models;
using Newtonsoft.Json;

namespace Funkmap.Module.Auth.Services.External
{
    public class GoogleAuthService : IExternalAuthService
    {
        private readonly HttpClient _httpClient;

        public GoogleAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public AuthProviderType ProviderType => AuthProviderType.Google;
        public async Task<UserEntity> BuildUserAsync(string token)
        {
            var uri = new Uri($"https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={token}");

            var facebookResponse = await _httpClient.GetAsync(uri);

            if (!facebookResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var facebookUserJson = await facebookResponse.Content.ReadAsStringAsync();
            var facebookUser = JsonConvert.DeserializeObject<GoogleUser>(facebookUserJson);

            var userEntity = new UserEntity
            {
                AvatarId = facebookUser.PictureUrl,
                Email = facebookUser.Email,
                Login = facebookUser.Email,
                Name = facebookUser.Name
            };

            return userEntity;
        }
    }
}
