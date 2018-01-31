using Microsoft.Build.Framework;

namespace Funkmap.Module.Auth.Models
{
    public class RegistrationRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Locale { get; set; }
    }
}
