using System.ComponentModel.DataAnnotations;

namespace Funkmap.Auth.Contracts
{
    /// <summary>
    /// Confirmation restore password model
    /// </summary>
    public class ConfirmRestoreRequest
    {
        /// <summary>
        /// Login or email
        /// </summary>
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
