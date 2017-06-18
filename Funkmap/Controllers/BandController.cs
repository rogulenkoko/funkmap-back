using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/band")]
    public class BandController: ApiController
    {
        private readonly IBandRepository _bandRepository;

        public BandController(IBandRepository bandRepository)
        {
            _bandRepository = bandRepository;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetBands()
        {
            var allBands = await _bandRepository.GetAllAsync();

            return Content(HttpStatusCode.OK, allBands);

        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetBand(string id)
        {
            var bandEntity = await _bandRepository.GetAsync(id);
            var musican = bandEntity.ToModel();
            return Content(HttpStatusCode.OK, musican);

        }
    }
}
