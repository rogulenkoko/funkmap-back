using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;

namespace Funkmap.Controllers
{

    [RoutePrefix("api/rehearsal")]
    public class RehearsalPointController : ApiController
    {
        private readonly IRehearsalPointRepository _rehearsalRepository;

        public RehearsalPointController(IRehearsalPointRepository rehearsalRepository)
        {
            _rehearsalRepository = rehearsalRepository;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetRehearsalPoint(string id)
        {
            var rehearsalEntity = await _rehearsalRepository.GetAsync(id);
            var rehearsal = rehearsalEntity.ToPreviewModel();
            return Content(HttpStatusCode.OK, rehearsal);
        }
    }
}
