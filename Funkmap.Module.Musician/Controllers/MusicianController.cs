using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Module.Musician.Abstract;

namespace Funkmap.Module.Musician.Controllers
{
    [RoutePrefix("api/musician")]
    public class MusicianController: ApiController
    {
        private readonly IMusicianRepository _musicianRepository;

        public MusicianController(IMusicianRepository musicianRepository)
        {
            _musicianRepository = musicianRepository;
        }


        [HttpPost]
        [Route("all")]
        public async Task<IHttpActionResult> GetMusicians()
        {
            var allMusicians = await _musicianRepository.GetAllAsync();
            return Content(HttpStatusCode.OK, allMusicians);

        }
    }
}
