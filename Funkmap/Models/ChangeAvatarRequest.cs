using Funkmap.Common;

namespace Funkmap.Models
{
    public class ChangeAvatarRequest
    {
        public EntityType EntityType { get; set; }
        public string Login { get; set; }
        public byte[] Avatar { get; set; }
    }
}
