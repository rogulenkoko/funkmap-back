using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Models;
using Funkmap.Domain;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Domain.Services.Abstract;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Models.Requests;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/band")]
    public class BandController: ApiController
    {
        private readonly IBaseRepository _baseRepository;
        private readonly IDependenciesController _dependenciesController;
        
        public BandController(IBaseRepository baseRepository, IDependenciesController dependenciesController)
        {
            _baseRepository = baseRepository;
            _dependenciesController = dependenciesController;
        }
        

        /// <summary>
        /// Информация о группах, в которые можно пригласить музыканта 
        /// (музыкант не состоит в ней или еще не приглашен)
        /// </summary>
        /// <param name="request">Логин приглашаемого музыканта</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("getInviteInfo")]
        public async Task<IHttpActionResult> GetInviteMusicianInfo(BandInviteInfoRequest request)
        {
            var login = Request.GetLogin();

            var parameter = new CommonFilterParameter
            {
                EntityType = EntityType.Band,
                UserLogin = login,
                Skip = 0,
                Take = Int32.MaxValue
            };

            var bandEntities = await _baseRepository.GetFilteredAsync(parameter);
            var availableBands = bandEntities.Cast<Band>()
                .Where(x=>(x.Musicians == null && x.InvitedMusicians == null) 
                    || ((x.Musicians == null || !x.Musicians.Contains(request.InvitedMusician))) 
                    && (x.InvitedMusicians == null || !x.InvitedMusicians.Contains(request.InvitedMusician)))
                .Select(x=>x.ToPreviewModel()).ToList();

            var info = new BandInviteInfo()
            {
                AvailableBands = availableBands
            };

            return Ok(info);
        }

        /// <summary>
        /// Удаление музыканта из группы
        /// </summary>
        /// <param name="membersRequest">Логин группы (из которой надо удалить музыканта) и логин музыканта</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("removeMusician")]
        public async Task<IHttpActionResult> RemoveMusicianFromBand(UpdateBandMemberRequest membersRequest)
        {
            var userLogin = Request.GetLogin();
            var band = await _baseRepository.GetAsync(membersRequest.BandLogin);
            if (band.UserLogin != userLogin) return BadRequest("is not your band");

            var parameter = new CleanDependenciesParameter()
            {
                EntityType = EntityType.Musician,
                EntityLogin = membersRequest.MusicianLogin,
                FromEntityLogin = membersRequest.BandLogin
            };

            await _dependenciesController.CleanDependenciesAsync(parameter);

            return Ok(new BaseResponse() { Success = true });
        }
    }
}
