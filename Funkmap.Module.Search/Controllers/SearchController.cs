using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Abstract.Search;
using Funkmap.Module.Search.Extensions;
using Funkmap.Module.Search.Models;

namespace Funkmap.Module.Search.Controllers
{
    [RoutePrefix("api/search")]
    public class SearchController : ApiController
    {
        private readonly IEnumerable<ISearchService> _searchServices;
        public SearchController(IEnumerable<ISearchService> searchServices)
        {
            _searchServices = searchServices;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetAll()
        {
            var searchTasks = _searchServices.Select(x=>x.SearchAllAsync()).ToArray();

            Task.WaitAll(searchTasks);

            var result = searchTasks.Select(x => x.Result).SelectMany(x => x);

            return Content(HttpStatusCode.OK, result);

        }

        [HttpGet]
        [Route("nearest")]
        public async Task<IHttpActionResult> GetNearest(SearchRequest request)
        {
            var searchTasks = _searchServices.Select(x => x.SearchNearest(request)).ToArray();

            Task.WaitAll(searchTasks);

            var result = searchTasks.Select(x => x.Result).SelectMany(x => x).ToList().SortByLocationToPoint(request.Longitude, request.Latitude);

            return Content(HttpStatusCode.OK, result);

        }
    }
}
