using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Filters;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models.Requests;
using Funkmap.Models.Responses;
using Funkmap.Services.Abstract;
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
        private readonly IEventBus _eventBus;

        public BaseController(IBaseRepository repository,
                              IParameterFactory parameterFactory,
                              IEntityUpdateService updateService,
                              IDependenciesController dependenciesController,
                              IEventBus eventBus)
        {
            _repository = repository;
            _parameterFactory = parameterFactory;
            _updateService = updateService;
            _dependenciesController = dependenciesController;
            _eventBus = eventBus;
        }


        /// <summary>
        /// Ближайшие n профилей (информация о навигации) по отношению к указанной точке
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
            var markers = result.Select(x => x.ToMarkerModel());
            return Content(HttpStatusCode.OK, markers);

        }

        /// <summary>
        /// Ближайшие n профилей (основная информация) по отношению к указанной точке
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
            List<BaseEntity> result = await _repository.GetFullNearestAsync(parameters);
            var searchModels = result.Select(x => x.ToSearchModel());
            return Content(HttpStatusCode.OK, searchModels);
        }


        /// <summary>
        /// Информация о навигации некоторых профилях
        /// </summary>
        /// <param name="logins">Логины профилей</param>
        /// <returns></returns>
        [HttpPost]
        [Route("specificmarkers")]
        public async Task<IHttpActionResult> GetSpecificMarkers(string[] logins)
        {
            var baseEntities = await _repository.GetSpecificNavigationAsync(logins);
            var items = baseEntities.Select(x => x.ToMarkerModel());
            return Ok(items);
        }

        /// <summary>
        /// Информация о навигации фильтрованных профилей
        /// </summary>
        /// <param name="request">Параметры фильтрации</param>
        /// <returns></returns>
        [HttpPost]
        [Route("filteredmarkers")]
        public async Task<IHttpActionResult> GetFilteredMarkers(FilteredRequest request)
        {

            var commonParameter = new CommonFilterParameter
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

            var baseEntities = await _repository.GetFilteredNavigationAsync(commonParameter, paramter);
            var items = baseEntities.Select(x => x.ToMarkerModel());
            return Ok(items);
        }

        /// <summary>
        /// Основная информация о порции отфильтрованных профилей, логины всех отфильтрованных профилей
        /// </summary>
        /// <param name="request">Параметры фильтрации</param>
        /// <returns></returns>
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
                Limit = request.Limit,
                UserLogin = request.UserLogin
            };
            var paramter = _parameterFactory.CreateParameter(request);
            var filteredEntities = await _repository.GetFilteredAsync(commonParameter, paramter);
            var items = filteredEntities.Select(x => x.ToSearchModel()).ToList();

            //todo сравнить что быстрее, делать два запроса или агрегирующий запрос
            var count = await _repository.GetAllFilteredCountAsync(commonParameter, paramter);

            var reponse = new SearchResponse()
            {
                Items = items,
                AllCount = count
            };
            return Ok(reponse);
        }
    }
}
