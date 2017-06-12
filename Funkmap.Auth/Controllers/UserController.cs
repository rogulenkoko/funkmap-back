using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Auth.Data.Abstract;
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
            var response = new BaseResponse();
            response.Success = await _authRepository.SaveAvatarAsync(request.Login, request.Avatar);
            return Ok(response);
        }
    }
}
