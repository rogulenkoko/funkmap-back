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
    [RoutePrefix("api/studio")]
    public class StudioController : ApiController
    {
        private readonly IStudioRepository _studioRepository;

        public StudioController(IStudioRepository studioRepository)
        {
            _studioRepository = studioRepository;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetStudio(string id)
        {
            var studioEntity = await _studioRepository.GetAsync(id);
            var studio = studioEntity.ToPreviewModel();
            return Content(HttpStatusCode.OK, studio);
        }
    }
}
