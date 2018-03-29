using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Filters;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Domain.Services.Abstract;
using Funkmap.Mappers;
using Funkmap.Models.Requests;
using Funkmap.Models.Responses;
using Funkmap.Tools.Abstract;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/base")]
    [ValidateRequestModel]
    public partial class BaseController : ApiController
    {
        private readonly IBaseRepository _repository;
        private readonly IParameterFactory _parameterFactory;
        private readonly IUpdateRepository _updateRepository;
        private readonly IDependenciesController _dependenciesController;

        public BaseController(IBaseRepository repository,
                              IParameterFactory parameterFactory,
                              IUpdateRepository updateRepository,
                              IDependenciesController dependenciesController)
        {
            _repository = repository;
            _parameterFactory = parameterFactory;
            _updateRepository = updateRepository;
            _dependenciesController = dependenciesController;
        }


        /// <summary>
        /// Ближайшие n профилей (информация о навигации) по отношению к указанной точке
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(List<Marker>))]
        [Route("nearest")]
        public async Task<IHttpActionResult> GetNearest(LocationRequest request)
        {
            var parameters = new LocationParameter()
            {
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                RadiusKm = request.RadiusKm,
                Take = request.Limit
            };
            List<Marker> markers = await _repository.GetNearestMarkersAsync(parameters);

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
            var parameters = new LocationParameter
            {
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                RadiusKm = request.RadiusKm,
                Take = request.Take,
                Skip = request.Skip
            };
            List<SearchItem> searchModels = await _repository.GetNearestAsync(parameters);
            return Content(HttpStatusCode.OK, searchModels);
        }


        /// <summary>
        /// Информация о навигации некоторых профилях
        /// </summary>
        /// <param name="logins">Логины профилей</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(List<Marker>))]
        [Route("specificmarkers")]
        public async Task<IHttpActionResult> GetSpecificMarkers(string[] logins)
        {
            List<Marker> items = await _repository.GetSpecificMarkersAsync(logins);
            return Ok(items);
        }

        /// <summary>
        /// Информация о навигации фильтрованных профилей
        /// </summary>
        /// <param name="request">Параметры фильтрации</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(List<Marker>))]
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
                RadiusKm = request.RadiusDeg,
                Limit = request.Limit
            };
            var paramter = _parameterFactory.CreateParameter(request);

            List<Marker> items = await _repository.GetFilteredMarkersAsync(commonParameter, paramter);
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
                RadiusKm = request.RadiusDeg,
                Limit = request.Limit,
                UserLogin = request.UserLogin
            };
            var paramter = _parameterFactory.CreateParameter(request);
            var filteredEntities = await _repository.GetFilteredAsync(commonParameter, paramter);
            var items = filteredEntities.ToSearchItems();

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
