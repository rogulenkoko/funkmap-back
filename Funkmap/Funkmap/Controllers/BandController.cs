using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Funkmap.Common.Auth;
using Funkmap.Common.Models;
using Funkmap.Domain;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Models.Requests;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/band")]
    public class BandController : ApiController
    {
        private readonly IBaseQueryRepository _baseQueryRepository;
        private readonly IBaseCommandRepository _commandRepository;

        public BandController(IBaseQueryRepository baseQueryRepository, IBaseCommandRepository commandRepository)
        {
            _baseQueryRepository = baseQueryRepository;
            _commandRepository = commandRepository;
        }


        /// <summary>
        /// Get information about bands in which you can invite musicians.
        /// (Musician is not a participant of the band and haven't invited yet.)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ResponseType(typeof(BandInviteInfo))]
        [Route("invite/{login}")]
        public async Task<IHttpActionResult> GetInviteMusicianInfo(string login)
        {
            var userLogin = Request.GetLogin();

            var parameter = new CommonFilterParameter
            {
                EntityType = EntityType.Band,
                UserLogin = userLogin,
                Skip = 0,
                Take = Int32.MaxValue
            };

            var bandEntities = await _baseQueryRepository.GetFilteredAsync(parameter);
            var availableBands = bandEntities.Cast<Band>()
                .Where(x => (x.Musicians == null && x.InvitedMusicians == null)
                    || ((x.Musicians == null || !x.Musicians.Contains(login)))
                    && (x.InvitedMusicians == null || !x.InvitedMusicians.Contains(login)))
                .Select(x => x.ToPreviewModel()).ToList();

            var info = new BandInviteInfo()
            {
                AvailableBands = availableBands
            };

            return Ok(info);
        }

        /// <summary>
        /// Remove musician from band.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(BaseResponse))]
        [Route("remove-musician")]
        public async Task<IHttpActionResult> RemoveMusicianFromBand(UpdateBandMemberRequest membersRequest)
        {
            var userLogin = Request.GetLogin();
            var band = await _baseQueryRepository.GetAsync<Band>(membersRequest.BandLogin);

            var bandUpdate = new Band
            {
                Login = band.Login,
                EntityType = band.EntityType,
                Musicians = band.Musicians.Except(new List<string>() { membersRequest.MusicianLogin }).ToList()
            };

            var parameter = new CommandParameter<Profile>
            {
                UserLogin = userLogin,
                Parameter = bandUpdate
            };

            var result = await _commandRepository.UpdateAsync(parameter);

            return Ok(new BaseResponse() { Success = result.Success, Error = result.Error });
        }
    }
}
