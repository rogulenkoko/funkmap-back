using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Models;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Tools;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/studio")]
    public class StudioController : ApiController
    {
        private readonly IStudioRepository _studioRepository;

        public StudioController(IStudioRepository studioRepository)
        {
            _studioRepository = studioRepository;
        }

        [HttpGet]
        [Route("getFull/{id}")]
        public async Task<IHttpActionResult> GetFullStudio(string id)
        {
            var studioEntity = await _studioRepository.GetAsync(id);
            StudioModel studio = studioEntity.ToModel();
            return Content(HttpStatusCode.OK, studio);

        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetStudio(string id)
        {
            var studioEntity = await _studioRepository.GetAsync(id);
            var studio = studioEntity.ToPreviewModel();
            return Content(HttpStatusCode.OK, studio);
        }

        [Authorize]
        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> SaveMusician(StudioModel model)
        {
            var entity = model.ToStudioEntity();
            var response = new BaseResponse();

            var existingStudio = await _studioRepository.GetAsync(model.Login);
            if (existingStudio != null)
            {
                return Content(HttpStatusCode.OK, response);
            }

            var userLogin = Request.GetLogin();
            entity.UserLogin = userLogin;

            await _studioRepository.CreateAsync(entity);
            response.Success = true;
            return Content(HttpStatusCode.OK, response);

        }
    }
}
