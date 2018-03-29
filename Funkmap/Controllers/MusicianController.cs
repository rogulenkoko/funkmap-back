using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Domain;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Domain.Services.Abstract;
using Funkmap.Models.Requests;
using Funkmap.Notifications.Contracts.Specific.BandInvite;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/musician")]
    [ValidateRequestModel]
    public class MusicianController: ApiController
    {
        private readonly IBaseRepository _baseRepository;
        private readonly IFunkmapNotificationService _notificationService;
        private readonly IBandUpdateService _bandUpdateService;
        private readonly IDependenciesController _dependenciesController;

        public MusicianController(IBaseRepository baseRepository,
                                  IFunkmapNotificationService notificationService,
                                  IBandUpdateService bandUpdateService,
                                  IDependenciesController dependenciesController)
        {
            _baseRepository = baseRepository;
            _notificationService = notificationService;
            _bandUpdateService = bandUpdateService;
            _dependenciesController = dependenciesController;
        }

        /// <summary>
        /// Приглашение музыканта в группу
        /// </summary>
        /// <param name="membersRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("invite")]
        public async Task<IHttpActionResult> InviteMusician(UpdateBandMemberRequest membersRequest)
        {
            if (String.IsNullOrEmpty(membersRequest.BandLogin) || String.IsNullOrEmpty(membersRequest.MusicianLogin))
            {
                return BadRequest("ivalid membersRequest parameter");
            }

            var login = Request.GetLogin();

            var parameter = new UpdateBandMemberParameter
            {
                MusicianLogin = membersRequest.MusicianLogin,
                BandLogin = membersRequest.BandLogin
            };

            InviteBandResponse inviteResponse = await _bandUpdateService.HandleInviteBandChanges(parameter, login);
            
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

        /// <summary>
        /// Приглашение музыкантов в группу
        /// </summary>
        /// <param name="membersRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("inviteMany")]
        public async Task<IHttpActionResult> InviteManyMusician(UpdateBandMembersRequest membersRequest)
        {
            if (String.IsNullOrEmpty(membersRequest.BandLogin) || membersRequest.MusicianLogins == null || membersRequest.MusicianLogins.Count == 0)
            {
                return BadRequest("ivalid membersRequest parameter");
            }

            var login = Request.GetLogin();


            var response = new List<InviteBandResponse>();

            foreach (var musicianLogin in membersRequest.MusicianLogins)
            {
                var parameter = new UpdateBandMemberParameter
                {
                    MusicianLogin = musicianLogin,
                    BandLogin = membersRequest.BandLogin
                };

                InviteBandResponse inviteResponse = await _bandUpdateService.HandleInviteBandChanges(parameter, login);

                if (!inviteResponse.IsOwner)
                {
                    var requestMessage = new BandInviteNotification()
                    {
                        BandLogin = membersRequest.BandLogin,
                        InvitedMusicianLogin = musicianLogin,
                        SenderLogin = login,
                        RecieverLogin = inviteResponse.OwnerLogin,
                        BandName = inviteResponse.BandName,
                    };

                    _notificationService.NotifyBandInvite(requestMessage);
                }

                response.Add(inviteResponse);
            }
            
            return Ok(response);
        }

        /// <summary>
        /// Удаление музыканта из группы
        /// </summary>
        /// <param name="membersRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("leaveBand")]
        public async Task<IHttpActionResult> LeaveBand(UpdateBandMemberRequest membersRequest)
        {
            var userLogin = Request.GetLogin();
            var musician = await _baseRepository.GetAsync(membersRequest.MusicianLogin);
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
