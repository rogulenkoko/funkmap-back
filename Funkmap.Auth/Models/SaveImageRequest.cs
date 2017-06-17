
using Microsoft.Build.Framework;

namespace Funkmap.Module.Auth.Models
{
    public class SaveImageRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public byte[] Avatar { get; set; }
    }
}
