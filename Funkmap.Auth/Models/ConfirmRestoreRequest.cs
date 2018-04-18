using Microsoft.Build.Framework;

namespace Funkmap.Module.Auth.Models
{
    public class ConfirmRestoreRequest
    {
        [Required]
        public string LoginOrEmail { get; set; }

        /// <summary>
        /// Confirmation code
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// New password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
