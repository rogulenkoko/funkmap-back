using System.ComponentModel.DataAnnotations;

namespace Funkmap.Auth.Contracts
{
    public class ConfirmationRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Confirmation code from email
        /// </summary>
        [Required]
        public string Code { get; set; }
    }
}
