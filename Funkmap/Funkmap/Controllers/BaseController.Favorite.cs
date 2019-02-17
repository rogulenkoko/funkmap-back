using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Core.Auth;
using Funkmap.Common.Models;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Models.Requests;
using Funkmap.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funkmap.Controllers
{
    public partial class BaseController
    {
        /// <summary>
        /// Get marked as favourite profile's base inforamation.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("favorites")]
        public async Task<IActionResult> GetFavorites()
        {
            var login = User.GetLogin();
            List<string> favoritesLogins = await _queryRepository.GetFavoritesLoginsAsync(login);
            List<SearchItem> favorites = await _queryRepository.GetSpecificAsync(favoritesLogins);
            Request.SetProfilesCorrectAvatarUrls(favorites);
            return Ok(favorites);
        }

        /// <summary>
        /// Get marked as favourite profile's logins.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("favorites/logins")]
        public async Task<IActionResult> GetFavoritesLogins()
        {
            var login = User.GetLogin();
            var favoritesLogins = await _queryRepository.GetFavoritesLoginsAsync(login);
            return Ok(favoritesLogins);
        }

        /// <summary>
        /// Add or delete favourite profile.
        /// </summary>
        /// <param name="request"><see cref="UpdateFavoriteRequest"/></param>
        [Authorize]
        [HttpPost]//todo correct method
        [Route("favorites")]
        public async Task<IActionResult> UpdateFavorite([FromBody]UpdateFavoriteRequest request)
        {
            var login = User.GetLogin();
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
