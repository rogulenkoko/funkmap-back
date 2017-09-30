using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Funkmap.Common.Auth;
using Funkmap.Common.Models;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Models.Requests;
using Funkmap.Models.Responses;
using Funkmap.Services;
using Funkmap.Tools;
using Funkmap.Tools.Abstract;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/base")]
    public class BaseController : ApiController
    {
        private readonly IBaseRepository _repository;
        private readonly IParameterFactory _parameterFactory;
        private readonly IEntityUpdateService _updateService;

        public BaseController(IBaseRepository repository,
                              IParameterFactory parameterFactory,
                              IEntityUpdateService updateService)
        {
            _repository = repository;
            _parameterFactory = parameterFactory;
            _updateService = updateService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _repository.GetAllAsyns();
            var markers = result.Select(x => x.ToMarkerModel()).ToList();
            return Content(HttpStatusCode.OK, markers);

        }

        [HttpPost]
        [Route("nearest")]
        public async Task<IHttpActionResult> GetNearest(LocationRequest request)
        {
            var parameters = new LocationParameter()
            {
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                RadiusDeg = request.RadiusDeg,
                Take = request.Limit
            };
            var result = await _repository.GetNearestAsync(parameters);
            var markers = result.Select(x => x.ToMarkerModel()).ToList();
            return Content(HttpStatusCode.OK, markers);

        }

        [HttpPost]
        [Route("fullnearest")]
        public async Task<IHttpActionResult> GetFullNearest(FullLocationRequest request)
        {
            var parameters = new LocationParameter()
            {
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                RadiusDeg = request.RadiusDeg,
                Take = request.Take,
                Skip = request.Skip
            };
            var result = await _repository.GetFullNearestAsync(parameters);
            var searchModels = result.Select(x => x.ToSearchModel()).ToList();
            return Content(HttpStatusCode.OK, searchModels);
        }

        [HttpPost]
        [Route("specific")]
        public async Task<IHttpActionResult> GetSpecific(string[] logins)
        {
            var baseEntities = await _repository.GetSpecificNavigationAsync(logins);
            var items = baseEntities.Select(x => x.ToSearchModel());
            return Ok(items);
        }

        [HttpPost]
        [Route("specificmarkers")]
        public async Task<IHttpActionResult> GetSpecificMarkers(string[] logins)
        {
            var baseEntities = await _repository.GetSpecificNavigationAsync(logins);
            var items = baseEntities.Select(x => x.ToMarkerModel());
            return Ok(items);
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

        [HttpPost]
        [Route("filtered")]
        public async Task<IHttpActionResult> GetFiltered(FilteredRequest request)
        {
            var commonParameter = new CommonFilterParameter()
            {
                EntityType = request.EntityType,
                SearchText = request.SearchText,
                Skip = request.Skip,
                Take = request.Take,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                RadiusDeg = request.RadiusDeg
            };
            var paramter = _parameterFactory.CreateParameter(request);
            var filteredEntities = await _repository.GetFilteredAsync(commonParameter, paramter);
            var items = filteredEntities.Select(x => x.ToSearchModel()).ToList();

            var logins = await _repository.GetAllFilteredLoginsAsync(commonParameter, paramter);

            var reponse = new SearchResponse()
            {
                Items = items,
                AllCount = logins.Count,
                AllLogins = logins
            };
            return Ok(reponse);
        }

        [HttpGet]
        [Route("checkLogin/{login}")]
        public async Task<IHttpActionResult> CheckIfLoginExist(string login)
        {
            var isExist = await _repository.CheckIfLoginExistAsync(login);
            return Ok(isExist);
        }

        [HttpPost]
        [Route("update")]
        [Authorize]
        public async Task<IHttpActionResult> Update([ModelBinder(typeof(FunkmapModelBinderProvider))]BaseModel model)
        {
            await _updateService.UpdateEntity(model);
            return Ok(new BaseResponse() { Success = true });
        }

        [HttpGet]
        [Route("delete/{login}")]
        [Authorize]
        public async Task<IHttpActionResult> Delete(string login)
        {
            var array = new [] { login };
            var musicianEntities = await _repository.GetSpecificNavigationAsync(array);
            var musician = musicianEntities.FirstOrDefault();
            if (musician == null) return BadRequest("entity doesn't exist");

            var userLogin = Request.GetLogin();
            if (musician.UserLogin != userLogin) return BadRequest("is not your entity");

            await _repository.DeleteAsync(login);

            return Ok(new BaseResponse() { Success = true });
        }

    }
}
