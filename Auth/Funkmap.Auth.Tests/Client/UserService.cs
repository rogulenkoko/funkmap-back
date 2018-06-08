using System;
using System.Net.Http;
using System.Threading.Tasks;
using Funkmap.Auth.Contracts;
using Newtonsoft.Json;

namespace Funkmap.Auth.Tests.Client
{
    public class UserService
    {
        private readonly string _url;
        public UserService(string url)
        {
            _url = $"{url}/api/user";
        }

        public async Task<UserResponse> GetUserAsync(string login)
        {
            using (var client = new HttpClient())
            {
                var userUrl = $"{_url}/{login}";
                var response = await client.GetAsync(userUrl);
                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"Can't execute request to {userUrl}.");
                }
                var userJson = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserResponse>(userJson);
                return user;
            }
        } 
    }
}