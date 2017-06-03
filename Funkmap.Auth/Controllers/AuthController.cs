using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Auth.Data.Abstract;

namespace Funkmap.Module.Auth.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login(Credantials creds)
        {
            var isExist = _authRepository.Login(creds.Login, creds.Password);
            if (isExist)
            {

            }

            return Ok();
        }
    }

    public class Credantials
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
