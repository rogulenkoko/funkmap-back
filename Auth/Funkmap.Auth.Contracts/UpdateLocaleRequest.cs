using System.ComponentModel.DataAnnotations;

namespace Funkmap.Auth.Contracts
{
    public class UpdateLocaleRequest
    {
        [Required]
        public string Locale { get; set; }
    }
}
