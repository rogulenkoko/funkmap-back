using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Funkmap.Auth.Domain.Abstract;
using Funkmap.Auth.Domain.Models;
using Funkmap.Common.Auth;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Module.Auth.Models;

namespace Funkmap.Module.Auth.Controllers
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

        [HttpGet]
        [ResponseType(typeof(UserResponse))]
        [Route("user/{login}")]
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


        [HttpGet]
        [Route("avatar/{login}")]
        public async Task<IHttpActionResult> GetAvatar(string login)
        {
            var image = await _authRepository.GetAvatarAsync(login);
            return Ok(image);
        }

        [HttpPost]
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
