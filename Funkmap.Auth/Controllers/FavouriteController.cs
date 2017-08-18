using System;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Common.Auth;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;

namespace Funkmap.Module.Auth.Controllers
{
    [RoutePrefix("api/favourites")]
    [ValidateRequestModel]
    public class FavouriteController : ApiController
    {
        private readonly IAuthRepository _authRepository;

        public FavouriteController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        
        [HttpGet]
        [Authorize]
        [Route("logins")]
        public async Task<IHttpActionResult> GetFavouritesLogins()
        {
            var login = Request.GetLogin();
            var result = await _authRepository.GetFavouritesAsync(login);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [Route("setfavourite/{favouriteLogin}")]
        public async Task<IHttpActionResult> SetFavourite(string favouriteLogin)
        {
            var response = new BaseResponse();
            var login = Request.GetLogin();

            await _authRepository.SetFavourite(login, favouriteLogin);
            response.Success = true;
            
            return Ok(response);
        }
    }
}
