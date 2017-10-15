using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Funkmap.Common.Auth;
using Funkmap.Common.Models;
using Funkmap.Data.Parameters;
using Funkmap.Models;
using Funkmap.Models.Requests;
using Funkmap.Tools;

namespace Funkmap.Controllers
{
    public partial class BaseController
    {
        [Authorize]
        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> SaveMusician([ModelBinder(typeof(FunkmapModelBinderProvider))]BaseModel model)
        {
            var login = Request.GetLogin();
            model.UserLogin = login;
            await _updateService.CreateEntity(model);
            return Ok(new BaseResponse() { Success = true });

        }

        [HttpPost]
        [Route("update")]
        [Authorize]
        public async Task<IHttpActionResult> Update([ModelBinder(typeof(FunkmapModelBinderProvider))]BaseModel model)
        {
            await _updateService.UpdateEntity(model);
            return Ok(new BaseResponse() { Success = true });
        }

        [HttpGet]
        [Route("delete/{login}")]
        [Authorize]
        public async Task<IHttpActionResult> Delete(string login)
        {
            var entity = await _repository.GetAsync(login);
            if (entity == null) return BadRequest("entity doesn't exist");

            var userLogin = Request.GetLogin();
            if (entity.UserLogin != userLogin) return BadRequest("is not your entity");

            var deleted = await _repository.DeleteAsync(login);
            _dependenciesController.CleanDeletedDependencies(deleted);

            return Ok(new BaseResponse() { Success = true });
        }

        [Authorize]
        [Route("updateFavorite")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateFavorite(UpdateFavoriteRequest request)
        {
            var login = Request.GetLogin();
            var parameter = new UpdateFavoriteParameter()
            {
                EntityLogin = request.EntityLogin,
                IsFavorite = request.IsFavorite,
                UserLogin = login
            };
            await _repository.UpdateFavoriteAsync(parameter);
            return Ok(new BaseResponse() {Success = true});
        }
    }
}
