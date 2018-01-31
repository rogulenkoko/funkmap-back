using Microsoft.Build.Framework;

namespace Funkmap.Module.Auth.Models
{
    public class UpdateLocaleRequest
    {
        [Required]
        public string Locale { get; set; }
    }
}
