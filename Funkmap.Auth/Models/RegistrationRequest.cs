using Microsoft.Build.Framework;

namespace Funkmap.Module.Auth.Models
{
    public class RegistrationRequest
    {
        [Required]
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
