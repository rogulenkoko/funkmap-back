using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common;
using Funkmap.Common.Auth;
using Funkmap.Common.Models;
using Funkmap.Contracts.Notifications;
using Funkmap.Data.Entities;
using Funkmap.Data.Parameters;
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
        private readonly IBaseRepository _baseRepository;


        public BandController(IBandRepository bandRepository, IBaseRepository baseRepository)
        {
            _bandRepository = bandRepository;
            _baseRepository = baseRepository;
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
        
        [HttpPost]
        [Authorize]
        [Route("getInviteInfo")]
        public async Task<IHttpActionResult> GetInviteMusicianInfo(BandInviteInfoRequest request)
        {
            var login = Request.GetLogin();

            var parameter = new CommonFilterParameter()
            {
                EntityType = EntityType.Band,
                UserLogin = login,
                Skip = 0,
                Take = Int32.MaxValue
            };
            var bandEntities = await _baseRepository.GetFilteredAsync(parameter);
            var availableBands = bandEntities.Cast<BandEntity>().Where(x=> x.InvitedMusicians == null || x.MusicianLogins == null || (!x.InvitedMusicians.Contains(request.InvitedMusician) && !x.MusicianLogins.Contains(request.InvitedMusician)))
                .Select(x=>x.ToModelPreview()).ToList();

            var info = new BandInviteInfo()
            {
                AvailableBands = availableBands
            };
            return Ok(info);
        }
    }
}
