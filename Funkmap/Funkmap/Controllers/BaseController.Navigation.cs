using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Domain.Services.Abstract;
using Funkmap.Mappers;
using Funkmap.Models.Requests;
using Funkmap.Models.Responses;
using Funkmap.Tools;
using Funkmap.Tools.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Funkmap.Controllers
{
    [Route("api/base")]
    public partial class BaseController : Controller
    {
        private readonly IBaseQueryRepository _queryRepository;
        private readonly IBaseCommandRepository _commandRepository;
        private readonly IParameterFactory _parameterFactory;
        private readonly IAccessService _accessService;

        public BaseController(IBaseQueryRepository queryRepository,
                              IBaseCommandRepository commandRepository,
                              IParameterFactory parameterFactory,
                              IAccessService accessService)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
            _parameterFactory = parameterFactory;
            _accessService = accessService;
        }


        /// <summary>
        /// Get nearest profiles base information.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("nearest")]
        public async Task<IActionResult> GetFullNearest(FullLocationRequest request)
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
            Request.SetProfilesCorrectAvatarUrls(searchModels);
            return Ok(searchModels);
        }


        /// <summary>
        /// Get nearest profiles navigation information.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("nearest/markers")]
        public async Task<ActionResult> GetNearest(LocationRequest request)
        {
            var parameters = new LocationParameter()
            {
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                RadiusKm = request.RadiusKm,
                Take = request.Limit
            };
            List<Marker> markers = await _queryRepository.GetNearestMarkersAsync(parameters);

            return Ok(markers);
        }
        

        /// <summary>
        /// Get profiles base information.
        /// </summary>
        /// <param name="request">Filtration parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("filtered")]
        public async Task<IActionResult> GetFiltered(FilteredRequest request)
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
            Request.SetProfilesCorrectAvatarUrls(items);
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
        [Route("filtered/markers")]
        public async Task<IActionResult> GetFilteredMarkers(FilteredRequest request)
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
        [HttpPost]
        [Route("specific")]
        public async Task<IActionResult> GetSpecific(string[] logins)
        {
            List<SearchItem> items = await _queryRepository.GetSpecificAsync(logins);
            Request.SetProfilesCorrectAvatarUrls(items);
            return Ok(items);
        }

        /// <summary>
        /// Get navigation information of specific profiles
        /// </summary>
        /// <param name="logins">Specefic profiles logins</param>
        /// <returns></returns>
        [HttpPost]
        [Route("specific/markers")]
        public async Task<IActionResult> GetSpecificMarkers(string[] logins)
        {
            List<Marker> items = await _queryRepository.GetSpecificMarkersAsync(logins);
            return Ok(items);
        }


    }
}
