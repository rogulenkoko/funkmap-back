using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models.Requests;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/base")]
    public class BaseController : ApiController
    {
        private readonly IBaseRepository _repository;

        public BaseController(IBaseRepository repository)
        {
            _repository = repository;
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
                RadiusDeg = request.RadiusDeg
            };
            var result = await _repository.GetNearestAsync(parameters);
            var markers = result.Select(x => x.ToMarkerModel()).ToList();
            return Content(HttpStatusCode.OK, markers);

        }

        [HttpPost]
        [Route("fullnearest")]
        public async Task<IHttpActionResult> GetFullNearest(FullLocationRequest request)
        {
            var parameters = new FullLocationParameter()
            {
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                RadiusDeg = request.RadiusDeg
            };
            var result = await _repository.GetFullNearestAsync(parameters);
            var searchModels = result.Select(x => x.ToSearchModel()).ToList();
            return Content(HttpStatusCode.OK, searchModels);
        }

        [HttpPost]
        [Route("specific")]
        public async Task<IHttpActionResult> GetSpecific(string[] logins)
        {
            var baseEntities = await _repository.GetSpecificAsync(logins);
            var items = baseEntities.Select(x => x.ToSearchModel());
            return Ok(items);

        }
    }
}
