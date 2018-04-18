using Microsoft.Build.Framework;

namespace Funkmap.Module.Auth.Models
{
    public class RegistrationRequest
    {
        /// <summary>
        /// Uniq user login
        /// </summary>
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// Not empty password
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Uniq user email
        /// </summary>
        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Locale { get; set; }
    }
}
