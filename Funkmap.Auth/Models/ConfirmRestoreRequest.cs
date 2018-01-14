using Microsoft.Build.Framework;

namespace Funkmap.Module.Auth.Models
{
    public class ConfirmRestoreRequest
    {
        [Required]
        public string LoginOrEmail { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
