using Microsoft.Build.Framework;

namespace Funkmap.Auth.Models
{
    public class UpdateLocaleRequest
    {
        [Required]
        public string Locale { get; set; }
    }
}
