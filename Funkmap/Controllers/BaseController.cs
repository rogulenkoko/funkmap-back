using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Models;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;

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
            return Content(HttpStatusCode.OK, result);

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
            return Content(HttpStatusCode.OK, result);

        }
    }
}
