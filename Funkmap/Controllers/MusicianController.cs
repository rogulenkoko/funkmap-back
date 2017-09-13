using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Filters;
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
    [RoutePrefix("api/musician")]
    [ValidateRequestModel]
    public class MusicianController: ApiController
    {
        private readonly IMusicianRepository _musicianRepository;
        private readonly IFunkmapNotificationService _notificationService;
        private readonly IBaseRepository _baseRepository;

        public MusicianController(IMusicianRepository musicianRepository,
                                  IBaseRepository baseRepository,
                                  IFunkmapNotificationService notificationService)
        {
            _musicianRepository = musicianRepository;
            _notificationService = notificationService;
            _baseRepository = baseRepository;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetMusician(string id)
        {
            var musicianEntity = await _musicianRepository.GetAsync(id);
            MusicianPreviewModel musican = musicianEntity.ToPreviewModel();
            return Content(HttpStatusCode.OK, musican);

        }

        [HttpGet]
        [Route("getFull/{id}")]
        public async Task<IHttpActionResult> GetFullMusician(string id)
        {
            var musicianEntity = await _musicianRepository.GetAsync(id);
            MusicianModel musican = musicianEntity.ToMusicianModel();
            return Content(HttpStatusCode.OK, musican);

        }

        [Authorize]
        [HttpPost]
        [Route("invite")]
        public async Task<IHttpActionResult> InviteMusician(BandInviteMusicianRequest request)
        {
            var login = Request.GetLogin();
            var musician = await _musicianRepository.GetAsync(request.MusicianLogin);
            if (musician == null) return BadRequest("musician doesn't exist");

            var recieverLogin = musician.UserLogin;

            var bandResult = await _baseRepository.GetSpecificAsync(new [] {request.BandLogin});
            var band = bandResult.SingleOrDefault() as BandEntity;
            band.InvitedMusicians.Add(musician.Login);
            _baseRepository.UpdateAsync(band);

            var requestMessage = new InviteToBandRequest()
            {
                BandLogin = request.BandLogin,
                InvitedMusicianLogin = request.MusicianLogin,
                SenderLogin = login,
                RecieverLogin = recieverLogin,
                BandName = band.Name
            };

            _notificationService.InviteMusicianToGroup(requestMessage);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> SaveMusician(MusicianModel model)
        {
            var entity = model.ToMusicianEntity();
            var response = new BaseResponse();

            var existingMusician = await _musicianRepository.GetAsync(model.Login);
            if (existingMusician != null)
            {
                return Content(HttpStatusCode.OK, response);
            }

            var userLogin = Request.GetLogin();
            entity.UserLogin = userLogin;

            await _musicianRepository.CreateAsync(entity);
            response.Success = true;
            return Content(HttpStatusCode.OK, response);

        }
    }
}
