using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Funkmap.Auth.Contracts;
using Funkmap.Auth.Domain.Abstract;
using Funkmap.Auth.Tools;
using Funkmap.Common.Models;
using Funkmap.Common.Owin.Auth;
using Funkmap.Common.Owin.Extensions;
using Funkmap.Common.Owin.Filters;

namespace Funkmap.Auth.Controllers
{
    /// <summary>
    /// Controller for user data
    /// </summary>
    [RoutePrefix("api/user")]
    [ValidateRequestModel]
    public class UserController : ApiController
    {
        private readonly IAuthRepository _authRepository;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="authRepository"><see cref="IAuthRepository"/></param>
        public UserController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        /// <summary>
        /// Get user's full information (if exists).
        /// </summary>
        /// <param name="login">Users's login</param>
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

            Request.SetProfileCorrectAvatarUrls(user);
            var response = new UserResponse
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
        [HttpGet]
        [Route("avatar/{login}")]
        public async Task<HttpResponseMessage> GetAvatar(string login)
        {
            var image = await _authRepository.GetAvatarAsync(login);
            return ControllerMedia.MediaResponse(image);
        }

        /// <summary>
        /// Update users's avatar.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        [Authorize]
        [Route("avatar")]
        public async Task<IHttpActionResult> SaveAvatar(SaveImageRequest request)
        {
            var login = Request.GetLogin();
            var path = await _authRepository.SaveAvatarAsync(login, request.Avatar);
            var response = new AvatarUpdateResponse
            {
                Success = true,
                AvatarPath = path
            };
            return Ok(response);
        }
    }
}
