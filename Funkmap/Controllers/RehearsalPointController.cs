using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Models;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Tools;

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
        [Route("getFull/{id}")]
        public async Task<IHttpActionResult> GetFullBand(string id)
        {
            var rehearsalPointEntity = await _rehearsalRepository.GetAsync(id);
            RehearsalPointModel band = rehearsalPointEntity.ToModel();
            return Content(HttpStatusCode.OK, band);

        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetRehearsalPoint(string id)
        {
            var rehearsalEntity = await _rehearsalRepository.GetAsync(id);
            var rehearsal = rehearsalEntity.ToPreviewModel();
            return Content(HttpStatusCode.OK, rehearsal);
        }

        [Authorize]
        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> SaveMusician(RehearsalPointModel model)
        {
            var entity = model.ToRehearsalPointEntity();
            var response = new BaseResponse();

            var existingRehearsal = await _rehearsalRepository.GetAsync(model.Login);
            if (existingRehearsal != null)
            {
                return Content(HttpStatusCode.OK, response);
            }

            var userLogin = Request.GetLogin();
            entity.UserLogin = userLogin;

            await _rehearsalRepository.CreateAsync(entity);
            response.Success = true;
            return Content(HttpStatusCode.OK, response);

        }
    }
}
