using System.Threading.Tasks;
using System.Web.Http;

namespace Funkmap.Module.Auth.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {

        public AuthController()
        {
            
        }

        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login(Credantials creds)
        {
            //if (CheckUser(username, password))
            //{
            //    return JwtService.GenerateToken(username);
            //}

            return Ok();
        }
    }

    public class Credantials
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
