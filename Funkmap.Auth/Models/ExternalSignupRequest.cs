
using Microsoft.Build.Framework;

namespace Funkmap.Module.Auth.Models
{
    public class ExternalSignupRequest
    {
        /// <summary>
        /// Токен авторизации стороннего сервиса
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// Тип стороннего сервиса (Facebook)
        /// </summary>
        [Required]
        public AuthProviderType ProviderType { get; set; }
    }
}
