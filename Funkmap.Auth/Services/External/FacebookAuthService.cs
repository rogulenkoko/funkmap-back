using System;
using System.Net.Http;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Entities;
using Funkmap.Module.Auth.Services.External.Abstract;
using Funkmap.Module.Auth.Services.External.Models;
using Newtonsoft.Json;

namespace Funkmap.Module.Auth.Services.External
{
    public class FacebookAuthService : IExternalAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthProviderType ProviderType => AuthProviderType.Facebook;

        public FacebookAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserEntity> BuildUserAsync(string token)
        {
            var uri = new Uri($"https://graph.facebook.com/me?access_token={token}&fields=id,name,email,picture");

            var facebookResponse = await _httpClient.GetAsync(uri);

            if (!facebookResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var facebookUserJson = await facebookResponse.Content.ReadAsStringAsync();
            var facebookUser = JsonConvert.DeserializeObject<FacebookUser>(facebookUserJson);

            var userEntity = new UserEntity()
            {
                AvatarId = facebookUser.Picture?.Data?.Url,
                Email = facebookUser.Email,
                Login = facebookUser.Email,
                Name = facebookUser.Name
            };

            return userEntity;
        }
    }
}
