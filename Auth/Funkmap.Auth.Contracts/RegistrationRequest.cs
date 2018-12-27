using System.ComponentModel.DataAnnotations;

namespace Funkmap.Auth.Contracts
{
    /// <summary>
    /// Registration request model
    /// </summary>
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

        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Locale
        /// </summary>
        [Required]
        public string Locale { get; set; }
    }
}
