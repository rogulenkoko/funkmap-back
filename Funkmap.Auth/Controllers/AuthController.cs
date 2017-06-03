using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Auth;
using Funkmap.Module.Auth.Models;
using Microsoft.Owin;

namespace Funkmap.Module.Auth.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthRepository _authRepository;

        private readonly ConcurrentDictionary<string, UserEntity> _usersConfirmationCache;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
            _usersConfirmationCache = new ConcurrentDictionary<string, UserEntity>();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> Register(RegistrationRequest creds)
        {
            var isExist = await _authRepository.CheckIfExist(creds.Login);
            isExist = isExist || _usersConfirmationCache.ContainsKey(creds.Login);

            var response = new RegistrationResponse()
            {
                Success = isExist
            };

            if (isExist)
            {
                _usersConfirmationCache[creds.Login] = new UserEntity() {Login = creds.Login, Password = creds.Password};
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("confirm")]
        public async Task<IHttpActionResult> Confirm(RegistrationRequest request)
        {
            var response = new RegistrationResponse();
            if (_usersConfirmationCache.ContainsKey(request.Login))
            {
                _usersConfirmationCache[request.Login].Email = request.Email;
            }

            return Ok(response);
        }
    }
}
