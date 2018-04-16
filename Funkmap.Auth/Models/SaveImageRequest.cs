
using Microsoft.Build.Framework;

namespace Funkmap.Module.Auth.Models
{
    public class SaveImageRequest
    {
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// Image bytes or base64 string
        /// </summary>
        [Required]
        public byte[] Avatar { get; set; }
    }
}
