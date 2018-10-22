using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Funkmap.Common.Models;
using Funkmap.Common.Owin.Auth;
using Funkmap.Common.Owin.Filters;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Notifications.BandInvite;
using Funkmap.Domain.Parameters;
using Funkmap.Domain.Services.Abstract;
using Funkmap.Models.Requests;
using Funkmap.Notifications.Contracts;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/musician")]
    [ValidateRequestModel]
    public class MusicianController : ApiController
    {
        private readonly IBaseQueryRepository _baseQueryRepository;
        private readonly IFunkmapNotificationService _notificationService;
        private readonly IBandUpdateService _bandUpdateService;
        private readonly IBaseCommandRepository _commandRepository;

        public MusicianController(IBaseQueryRepository baseQueryRepository,
                                  IFunkmapNotificationService notificationService,
                                  IBandUpdateService bandUpdateService,
                                  IBaseCommandRepository commandRepository)
        {
            _baseQueryRepository = baseQueryRepository;
            _notificationService = notificationService;
            _bandUpdateService = bandUpdateService;
            _commandRepository = commandRepository;
        }

        /// <summary>
        /// Invite musicians to the band.
        /// </summary>
        /// <param name="membersRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(List<InviteBandResponse>))]
        [Route("invite")]
        public async Task<IHttpActionResult> InviteManyMusician(UpdateBandMembersRequest membersRequest)
        {
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
                    var requestMessage = new BandInviteNotification
                    {
                        BandLogin = membersRequest.BandLogin,
                        InvitedMusicianLogin = musicianLogin,
                        BandName = inviteResponse.BandName,
                    };

                    await _notificationService.NotifyAsync(requestMessage, inviteResponse.OwnerLogin, login);
                }
                response.Add(inviteResponse);
            }

            return Ok(response);
        }

        /// <summary>
        /// Leave band (if musician is a participant of the band).
        /// </summary>
        /// <param name="membersRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(BaseResponse))]
        [Route("remove-band")]
        public async Task<IHttpActionResult> LeaveBand(UpdateBandMemberRequest membersRequest)
        {
            var userLogin = Request.GetLogin();
            var musician = await _baseQueryRepository.GetAsync<Musician>(membersRequest.MusicianLogin);

            var musicianUpdate = new Musician
            {
                Login = musician.Login,
                EntityType = musician.EntityType,
                BandLogins = musician.BandLogins.Except(new List<string>() { membersRequest.BandLogin }).ToList()
            };

            var parameter = new CommandParameter<Profile>()
            {
                UserLogin = userLogin,
                Parameter = musicianUpdate
            };

            var result = await _commandRepository.UpdateAsync(parameter);

            return Ok(new BaseResponse() { Success = result.Success, Error = result.Error });
        }
    }
}
