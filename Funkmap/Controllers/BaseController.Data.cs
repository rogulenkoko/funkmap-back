using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Data.Objects;
using Funkmap.Mappers;

namespace Funkmap.Controllers
{
    public partial class BaseController
    {
        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _repository.GetAllAsyns();
            var markers = result.Select(x => x.ToMarkerModel()).ToList();
            return Content(HttpStatusCode.OK, markers);
        }


        [HttpGet]
        [Route("getFull/{id}")]
        public async Task<IHttpActionResult> GetFullBand(string id)
        {
            var entity = await _repository.GetAsync(id);
            return Content(HttpStatusCode.OK, entity.ToSpecificModel());

        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetRehearsalPoint(string id)
        {
            var entity = await _repository.GetAsync(id);
            return Content(HttpStatusCode.OK, entity.ToSpecificPreviewModel());
        }

        [HttpPost]
        [Route("specific")]
        public async Task<IHttpActionResult> GetSpecific(string[] logins)
        {
            var baseEntities = await _repository.GetSpecificFullAsync(logins);
            var items = baseEntities.Select(x => x.ToSearchModel());
            return Ok(items);
        }

        [HttpPost]
        [Route("images")]
        public async Task<IHttpActionResult> GetImages(string[] ids)
        {
            var filteredIds = ids.Where(x => !String.IsNullOrEmpty(x)).ToArray();
            ICollection<FileInfo> files = await _repository.GetFilesAsync(filteredIds);
            return Ok(files);
        }

        [HttpGet]
        [Authorize]
        [Route("users")]
        public async Task<IHttpActionResult> GetUserEntitiesLogins()
        {
            var userLogin = Request.GetLogin();
            var logins = await _repository.GetUserEntitiesLoginsAsync(userLogin);
            return Ok(logins);
        }

        [HttpGet]
        [Authorize]
        [Route("userscount")]
        public async Task<IHttpActionResult> GetUserEntitiesCountInfo()
        {
            var userLogin = Request.GetLogin();
            var countResults = await _repository.GetUserEntitiesCountInfoAsync(userLogin);
            var result = countResults.ToCountModels();
            return Ok(result);
        }

        [HttpGet]
        [Route("checkLogin/{login}")]
        public async Task<IHttpActionResult> CheckIfLoginExist(string login)
        {
            var isExist = await _repository.CheckIfLoginExistAsync(login);
            return Ok(isExist);
        }

        [HttpGet]
        [Authorize]
        [Route("favoritesLogins")]
        public async Task<IHttpActionResult> GetFavoritesLogins()
        {
            var login = Request.GetLogin();
            var favoritesLogins = await _repository.GetFavoritesLoginsAsync(login);
            return Ok(favoritesLogins);
        }

        [HttpGet]
        [Authorize]
        [Route("favorites")]
        public async Task<IHttpActionResult> GetFavorites()
        {
            var login = Request.GetLogin();
            var favoritesLogins = await _repository.GetFavoritesLoginsAsync(login);
            var favorites = await _repository.GetSpecificFullAsync(favoritesLogins.ToArray());
            return Ok(favorites?.Select(x=>x.ToSearchModel()));
        }
    }
}
