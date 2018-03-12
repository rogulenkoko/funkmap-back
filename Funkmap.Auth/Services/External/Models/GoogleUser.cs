using Newtonsoft.Json;

namespace Funkmap.Module.Auth.Services.External.Models
{
    public class GoogleUser
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public string PictureUrl { get; set; }
    }
}
