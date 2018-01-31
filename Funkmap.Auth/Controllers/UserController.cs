using System;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Auth.Data.Abstract;
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
        [Route("user/{login}")]
        public async Task<IHttpActionResult> GetUser(string login)
        {
            if (String.IsNullOrEmpty(login)) return BadRequest("invalid login");
            var userEntity = await _authRepository.GetAsync(login);

            if (userEntity == null)
            {
                return Ok(new UserPreview() {IsExist = false});
            }

            var model = new UserPreview()
            {
                Login = userEntity.Login,
                Avatar = userEntity.AvatarId,
                Name = userEntity.Name,
                IsExist = true
            };

            return Ok(model);
        }


        [HttpGet]
        [Authorize]
        [Route("lastVisit")]
        public async Task<IHttpActionResult> UpdateLastVisitDate()
        {
            var response = new BaseResponse();
            var login = Request.GetLogin();
            await _authRepository.UpdateLastVisitDateAsync(login, DateTime.UtcNow);

            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        [Route("updateLocale")]
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
        [Route("saveAvatar")]
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
