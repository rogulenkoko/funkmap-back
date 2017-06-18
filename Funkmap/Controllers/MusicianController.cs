using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models;

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
            var musican = musicianEntity.ToMusicianModel();
            return Content(HttpStatusCode.OK, musican);

        }

        //[Authorize]
        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> SaveMusician(MusicianModel model)
        {
            var entity = model.ToMusicianEntity();
            var response = new BaseResponse();
            try
            {
                await _musicianRepository.CreateAsync(entity);
                response.Success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return Content(HttpStatusCode.OK, response);

        }
    }
}
