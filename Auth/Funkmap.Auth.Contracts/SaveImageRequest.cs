
using System.ComponentModel.DataAnnotations;

namespace Funkmap.Auth.Contracts
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
