using Newtonsoft.Json;

namespace Funkmap.Module.Auth.Models
{
    public class FacebookUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("picture")]
        public FacebookPicture Picture { get; set; }
    }

    public class FacebookPicture
    {
        [JsonProperty("data")]
        public FacebookPictureData Data { get; set; }
    }

    public class FacebookPictureData
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
