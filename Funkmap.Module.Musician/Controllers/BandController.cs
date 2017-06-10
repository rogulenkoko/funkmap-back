using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Module.Musician.Mappers;
using Funkmap.Module.Musician.Models;
using Funkmap.Musician.Data.Abstract;

namespace Funkmap.Module.Musician.Controllers
{
    [RoutePrefix("api/band")]
    public class BandController: ApiController
    {
        private readonly IBandRepository _bandRepository;

        public BandController(IBandRepository bandRepository)
        {
            _bandRepository = bandRepository;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetBands()
        {
            var allBands = await _bandRepository.GetAllAsync();

            return Content(HttpStatusCode.OK, allBands);

        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetBand(long id)
        {
            var bandEntity = await _bandRepository.GetAsync(id);
            var musican = bandEntity.ToModel();
            return Content(HttpStatusCode.OK, musican);

        }
    }
}
