using System;
using System.Collections.Generic;
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
            if (String.IsNullOrEmpty(request.BandLogin) || String.IsNullOrEmpty(request.MusicianLogin))
            {
                return BadRequest("ivalid request parameter");
            }

            var login = Request.GetLogin();
            var musician = await _musicianRepository.GetAsync(request.MusicianLogin);
            if (musician == null) return BadRequest("musician doesn't exist");

            var musicianOwnerLogin = musician.UserLogin;

            var bandResult = await _baseRepository.GetAsync(request.BandLogin);
            var band = bandResult as BandEntity;
            if (band == null) return BadRequest("no band");

            if ((band.InvitedMusicians != null && band.InvitedMusicians.Contains(musician.Login)) || (band.MusicianLogins != null && band.MusicianLogins.Contains(musician.Login)))
            {
                return Ok(new BaseResponse() { Success = false, Error = "musician is already in band or invited" });
            }

            if (musicianOwnerLogin == login)
            {
                if (band.MusicianLogins == null) band.MusicianLogins = new List<string>();
                band.MusicianLogins.Add(musician.Login);
                await _baseRepository.UpdateAsync(band);
                return Ok(new BaseResponse() { Success = true });
            }

            if (band.InvitedMusicians == null) band.InvitedMusicians = new List<string>();
            band.InvitedMusicians.Add(musician.Login);

            _baseRepository.UpdateAsync(band);

            var requestMessage = new InviteToBandRequest()
            {
                BandLogin = request.BandLogin,
                InvitedMusicianLogin = request.MusicianLogin,
                SenderLogin = login,
                RecieverLogin = musicianOwnerLogin,
                BandName = band.Name
            };

            _notificationService.InviteMusicianToGroup(requestMessage);
            var response = new BaseResponse() {Success = true};
            return Ok(response);
        }
    }
}
