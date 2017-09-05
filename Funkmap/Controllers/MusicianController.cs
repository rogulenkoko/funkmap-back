using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Tools;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/musician")]
    [ValidateRequestModel]
    public class MusicianController: ApiController
    {
        private readonly IMusicianRepository _musicianRepository;

        public MusicianController(IMusicianRepository musicianRepository)
        {
            _musicianRepository = musicianRepository;
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
