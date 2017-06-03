using System.Linq;
using System.Net.Http;
using System.Security.Claims;

namespace Funkmap.Common.Auth
{
    public static class AuthApiControllerExtension
    {
        public static string GetLogin(this HttpRequestMessage request)
        {
            return request.GetOwinContext().Authentication.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        }
    }
}
