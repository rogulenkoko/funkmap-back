using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Funkmap.Common.Auth;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Models.Requests;
using Funkmap.Models.Responses;
using Funkmap.Services;
using Funkmap.Services.Abstract;
using Funkmap.Tools;
using Funkmap.Tools.Abstract;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/base")]
    [ValidateRequestModel]
    public partial class BaseController : ApiController
    {
        private readonly IBaseRepository _repository;
        private readonly IParameterFactory _parameterFactory;
        private readonly IEntityUpdateService _updateService;
        private readonly IDependenciesController _dependenciesController;

        public BaseController(IBaseRepository repository,
                              IParameterFactory parameterFactory,
                              IEntityUpdateService updateService,
                              IDependenciesController dependenciesController)
        {
            _repository = repository;
            _parameterFactory = parameterFactory;
            _updateService = updateService;
            _dependenciesController = dependenciesController;
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
        [Route("specificmarkers")]
        public async Task<IHttpActionResult> GetSpecificMarkers(string[] logins)
        {
            var baseEntities = await _repository.GetSpecificNavigationAsync(logins);
            var items = baseEntities.Select(x => x.ToMarkerModel());
            return Ok(items);
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
                RadiusDeg = request.RadiusDeg,
                Limit = request.Limit
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
    }
}
