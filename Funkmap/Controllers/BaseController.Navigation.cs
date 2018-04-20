using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Funkmap.Common.Filters;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
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
        private readonly IBaseQueryRepository _queryRepository;
        private readonly IBaseCommandRepository _commandRepository;
        private readonly IParameterFactory _parameterFactory;

        public BaseController(IBaseQueryRepository queryRepository,
                              IBaseCommandRepository commandRepository,
                              IParameterFactory parameterFactory)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
            _parameterFactory = parameterFactory;
        }


        /// <summary>
        /// Get nearest profiles base information.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(List<SearchItem>))]
        [Route("nearest")]
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
            List<SearchItem> searchModels = await _queryRepository.GetNearestAsync(parameters);
            return Content(HttpStatusCode.OK, searchModels);
        }


        /// <summary>
        /// Get nearest profiles navigation information.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(List<Marker>))]
        [Route("nearest/markers")]
        public async Task<IHttpActionResult> GetNearest(LocationRequest request)
        {
            var parameters = new LocationParameter()
            {
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                RadiusKm = request.RadiusKm,
                Take = request.Limit
            };
            List<Marker> markers = await _queryRepository.GetNearestMarkersAsync(parameters);

            return Content(HttpStatusCode.OK, markers);
        }
        

        /// <summary>
        /// Get profiles base information.
        /// </summary>
        /// <param name="request">Filtration parameters</param>
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
            var filteredEntities = await _queryRepository.GetFilteredAsync(commonParameter, paramter);
            var items = filteredEntities.ToSearchItems();

            //todo сравнить что быстрее, делать два запроса или агрегирующий запрос
            var count = await _queryRepository.GetAllFilteredCountAsync(commonParameter, paramter);

            var reponse = new SearchResponse()
            {
                Items = items,
                AllCount = count
            };
            return Ok(reponse);
        }

        /// <summary>
        /// Get profiles navigation information.
        /// </summary>
        /// <param name="request">Filtration parameters</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(List<Marker>))]
        [Route("filtered/markers")]
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

            List<Marker> items = await _queryRepository.GetFilteredMarkersAsync(commonParameter, paramter);
            return Ok(items);
        }
        

        /// <summary>
        /// Get base information about specific profiles.
        /// </summary>
        /// <param name="logins">Profile's logins</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(List<SearchItem>))]
        [Route("specific")]
        public async Task<IHttpActionResult> GetSpecific(string[] logins)
        {
            List<SearchItem> items = await _queryRepository.GetSpecificAsync(logins);
            return Ok(items);
        }

        /// <summary>
        /// Get navigation information of specific profiles
        /// </summary>
        /// <param name="logins">Specefic profiles logins</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(List<Marker>))]
        [Route("specific/markers")]
        public async Task<IHttpActionResult> GetSpecificMarkers(string[] logins)
        {
            List<Marker> items = await _queryRepository.GetSpecificMarkersAsync(logins);
            return Ok(items);
        }


    }
}
