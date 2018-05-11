using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Funkmap.Auth.Domain.Abstract;
using Funkmap.Auth.Models;
using Funkmap.Common.Models;
using Funkmap.Common.Owin.Auth;
using Funkmap.Common.Owin.Filters;

namespace Funkmap.Auth.Controllers
{
    [RoutePrefix("api/user")]
    [ValidateRequestModel]
    public class UserController : ApiController
    {
        private readonly IAuthRepository _authRepository;

        public UserController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        /// <summary>
        /// Get user's full information (if exists).
        /// </summary>
        /// <param name="login">Users's login</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(UserResponse))]
        [Route("{login}")]
        public async Task<IHttpActionResult> GetUser(string login)
        {
            var user = await _authRepository.GetAsync(login);

            if (user == null)
            {
                return Ok(new UserResponse() { IsExists = false });
            }

            var response = new UserResponse()
            {
                IsExists = true,
                User = user
            };

            return Ok(response);
        }

        /// <summary>
        /// Update user locale (available: 'ru', 'en').
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("locale")]
        public async Task<IHttpActionResult> UpdateLocale(UpdateLocaleRequest request)
        {
            var response = new BaseResponse();
            var login = Request.GetLogin();
            await _authRepository.UpdateLocaleAsync(login, request.Locale);

            return Ok(response);
        }

        /// <summary>
        /// Get user's avatar (bytes or base64 string).
        /// </summary>
        /// <param name="login">Users's login</param>
        /// <returns></returns>
        [HttpGet]
        [Route("avatar/{login}")]
        public async Task<IHttpActionResult> GetAvatar(string login)
        {
            var image = await _authRepository.GetAvatarAsync(login);
            return Ok(image);
        }

        /// <summary>
        /// Update users's avatar.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("avatar")]
        public async Task<IHttpActionResult> SaveAvatar(SaveImageRequest request)
        {
            var response = new AvatarUpdateResponse();

            var path = await _authRepository.SaveAvatarAsync(request.Login, request.Avatar);
            response.Success = true;
            response.AvatarPath = path;
            return Ok(response);
        }
    }
}
