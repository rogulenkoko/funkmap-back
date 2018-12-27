using System.ComponentModel.DataAnnotations;

namespace Funkmap.Auth.Contracts
{
    /// <summary>
    /// Update locale request model
    /// </summary>
    public class UpdateLocaleRequest
    {
        /// <summary>
        /// Locale
        /// </summary>
        [Required]
        public string Locale { get; set; }
    }
}
