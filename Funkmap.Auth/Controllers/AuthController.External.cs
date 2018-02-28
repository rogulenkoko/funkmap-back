using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Models;
using Funkmap.Module.Auth.Models;

namespace Funkmap.Module.Auth.Controllers
{
    public partial class AuthController
    {
		/// <summary>
        /// Регистрация через сторонние сервисы (Facebook)
        /// </summary>
        /// <param name="request">Токен авторизации и тип провайдера</param>
        /// <returns></returns>
        [HttpPost]
        [Route("external")]
        public async Task<IHttpActionResult> ExternalSignup(ExternalSignupRequest request)
        {
            var user = await _externalAuthFacade.BuildUserAsync(request.Token, request.ProviderType);

            var result = await _contextManager.TryRegisterExternal(user);

            return Ok(new BaseResponse() { Success = result });
        }
    }
}
