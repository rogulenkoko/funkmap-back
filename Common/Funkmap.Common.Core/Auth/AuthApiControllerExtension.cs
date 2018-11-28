using System.Linq;
using System.Security.Claims;

namespace Funkmap.Common.Core.Auth
{
    /// <summary>
    /// Auth extensions
    /// </summary>
    public static class AuthApiControllerExtension
    {
        /// <summary>
        /// Get user's login from claims
        /// </summary>
        /// <param name="claims"><see cref="ClaimsPrincipal"/></param>
        public static string GetLogin(this ClaimsPrincipal claims)
        {
            return claims.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        }
    }
}
