using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Funkmap.Common.Models;
using Funkmap.Common.Owin.Auth;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Models.Requests;

namespace Funkmap.Controllers
{
    public partial class BaseController
    {
        /// <summary>
        /// Get marked as favourite profile's base inforamation.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(List<SearchItem>))]
        [Authorize]
        [Route("favorites")]
        public async Task<IHttpActionResult> GetFavorites()
        {
            var login = Request.GetLogin();
            List<string> favoritesLogins = await _queryRepository.GetFavoritesLoginsAsync(login);
            List<SearchItem> favorites = await _queryRepository.GetSpecificAsync(favoritesLogins);
            return Ok(favorites);
        }

        /// <summary>
        /// Get marked as favourite profile's logins.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("favorites/logins")]
        public async Task<IHttpActionResult> GetFavoritesLogins()
        {
            var login = Request.GetLogin();
            var favoritesLogins = await _queryRepository.GetFavoritesLoginsAsync(login);
            return Ok(favoritesLogins);
        }

        /// <summary>
        /// Add or delete favourite profile.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [Route("favorites")]
        [ResponseType(typeof(BaseResponse))]
        public async Task<IHttpActionResult> UpdateFavorite(UpdateFavoriteRequest request)
        {
            var login = Request.GetLogin();
            var parameter = new UpdateFavoriteParameter
            {
                ProfileLogin = request.EntityLogin,
                IsFavorite = request.IsFavorite,
                UserLogin = login
            };

            await _commandRepository.UpdateFavoriteAsync(parameter);
            return Ok(new BaseResponse() { Success = true });
        }
    }
}
