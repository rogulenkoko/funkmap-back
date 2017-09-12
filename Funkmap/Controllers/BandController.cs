using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Models;
using Funkmap.Contracts.Notifications;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Models.Requests;
using Funkmap.Services.Abstract;
using Funkmap.Tools;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/band")]
    public class BandController: ApiController
    {
        private readonly IBandRepository _bandRepository;
        private readonly IFunkmapNotificationService _notificationService;

        public BandController(IBandRepository bandRepository,
                              IFunkmapNotificationService notificationService)
        {
            _bandRepository = bandRepository;
            _notificationService = notificationService;
        }
        
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetBand(string id)
        {
            var bandEntity = await _bandRepository.GetAsync(id);
            var band = bandEntity.ToModelPreview();
            return Content(HttpStatusCode.OK, band);
        }

        [HttpGet]
        [Route("getFull/{id}")]
        public async Task<IHttpActionResult> GetFullBand(string id)
        {
            var bandEntity = await _bandRepository.GetAsync(id);
            BandModel band = bandEntity.ToModel();
            return Content(HttpStatusCode.OK, band);

        }

        [Authorize]
        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> SaveMusician(BandModel model)
        {
            var entity = model.ToBandEntity();
            var response = new BaseResponse();

            var existingBand = await _bandRepository.GetAsync(model.Login);
            if (existingBand != null)
            {
                return Content(HttpStatusCode.OK, response);
            }

            var userLogin = Request.GetLogin();
            entity.UserLogin = userLogin;

            await _bandRepository.CreateAsync(entity);
            response.Success = true;
            return Content(HttpStatusCode.OK, response);

        }

        [Authorize]
        [HttpPost]
        [Route("invite")]
        public IHttpActionResult InviteMusician(BandInviteMusicianRequest request)
        {
            var login = Request.GetLogin();
            var requestMessage = new InviteToBandRequest()
            {
                BandLogin = request.BandLogin,
                InvitedMusicianLogin = request.MusicianLogin,
                InviterLogin = login
            };

            _notificationService.InviteMusicianToGroup(requestMessage);
            return Ok();
        }
    }
}
