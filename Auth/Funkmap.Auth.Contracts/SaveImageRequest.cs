using System.ComponentModel.DataAnnotations;

namespace Funkmap.Auth.Contracts
{
    /// <summary>
    /// Save avatar request model
    /// </summary>
    public class SaveImageRequest
    {
        /// <summary>
        /// Image bytes or base64 string
        /// </summary>
        [Required]
        public byte[] Avatar { get; set; }
    }
}
