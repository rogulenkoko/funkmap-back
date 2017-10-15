using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
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
            var files = await _repository.GetFiles(filteredIds);
            return Ok(files);
        }

        [HttpGet]
        [Authorize]
        [Route("users")]
        public async Task<IHttpActionResult> GetUserEntitiesLogins()
        {
            var userLogin = Request.GetLogin();
            var logins = await _repository.GetUserEntitiesLogins(userLogin);
            return Ok(logins);
        }

        [HttpGet]
        [Authorize]
        [Route("userscount")]
        public async Task<IHttpActionResult> GetUserEntitiesCountInfo()
        {
            var userLogin = Request.GetLogin();
            var countResults = await _repository.GetUserEntitiesCountInfo(userLogin);
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
        [Route("favorites")]
        public async Task<IHttpActionResult> GetFavorites()
        {
            var login = Request.GetLogin();
            var favorites = await _repository.GetFavorites(login);
            return Ok(favorites);

        }
    }
}
