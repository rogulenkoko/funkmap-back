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
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetBand(string id)
        {
            var bandEntity = await _bandRepository.GetAsync(id);
            var band = bandEntity.ToModel();
            return Content(HttpStatusCode.OK, band);

        }
    }
}
