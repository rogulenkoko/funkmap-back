using System.ComponentModel.DataAnnotations;

namespace Funkmap.Auth.Contracts
{
    /// <summary>
    /// Confirmation request model
    /// </summary>
    public class ConfirmationRequest
    {
        /// <summary>
        /// Login
        /// </summary>
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Confirmation code from email
        /// </summary>
        [Required]
        public string Code { get; set; }
    }
}
