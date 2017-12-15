using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common;
using Funkmap.Common.Auth;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Models.Requests;
using Funkmap.Models.Responses;
using Funkmap.Notifications.Contracts.Specific;
using Funkmap.Notifications.Contracts.Specific.BandInvite;
using Funkmap.Services.Abstract;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/musician")]
    [ValidateRequestModel]
    public class MusicianController: ApiController
    {
        private readonly IMusicianRepository _musicianRepository;
        private readonly IFunkmapNotificationService _notificationService;
        private readonly IBandUpdateService _bandUpdateService;
        private readonly IDependenciesController _dependenciesController;

        public MusicianController(IMusicianRepository musicianRepository,
                                  IFunkmapNotificationService notificationService,
                                  IBandUpdateService bandUpdateService,
                                  IDependenciesController dependenciesController)
        {
            _musicianRepository = musicianRepository;
            _notificationService = notificationService;
            _bandUpdateService = bandUpdateService;
            _dependenciesController = dependenciesController;
        }

        [Authorize]
        [HttpPost]
        [Route("invite")]
        public async Task<IHttpActionResult> InviteMusician(UpdateBandMembersRequest membersRequest)
        {
            if (String.IsNullOrEmpty(membersRequest.BandLogin) || String.IsNullOrEmpty(membersRequest.MusicianLogin))
            {
                return BadRequest("ivalid membersRequest parameter");
            }

            var login = Request.GetLogin();

            InviteBandResponse inviteResponse = await _bandUpdateService.HandleInviteBandChanges(membersRequest, login);
            
            if (!inviteResponse.IsOwner)
            {
                var requestMessage = new BandInviteNotification()
                {
                    BandLogin = membersRequest.BandLogin,
                    InvitedMusicianLogin = membersRequest.MusicianLogin,
                    SenderLogin = login,
                    RecieverLogin = inviteResponse.OwnerLogin,
                    BandName = inviteResponse.BandName
                };

                _notificationService.NotifyBandInvite(requestMessage);
            }
            
            return Ok(inviteResponse);
        }

        [Authorize]
        [HttpPost]
        [Route("leaveBand")]
        public async Task<IHttpActionResult> LeaveBand(UpdateBandMembersRequest membersRequest)
        {
            var userLogin = Request.GetLogin();
            var musician = await _musicianRepository.GetAsync(membersRequest.MusicianLogin);
            if (musician.UserLogin != userLogin) return BadRequest("is not your musician");

            var parameter = new CleanDependenciesParameter()
            {
                EntityType = EntityType.Band,
                EntityLogin = membersRequest.BandLogin,
                FromEntityLogin = membersRequest.MusicianLogin
            };

            await _dependenciesController.CleanDependenciesAsync(parameter);

            return Ok(new BaseResponse() {Success = true});
        }
    }
}
