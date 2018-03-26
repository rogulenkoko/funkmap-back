using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Funkmap.Common.Auth;
using Funkmap.Common.Models;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Models.Requests;
using Funkmap.Tools;

namespace Funkmap.Controllers
{
    public partial class BaseController
    {
        /// <summary>
        /// Сохранение профиля
        /// </summary>
        /// <param name="model">Профиль</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> SaveMusician([ModelBinder(typeof(FunkmapModelBinderProvider))]Profile model)
        {
            var login = Request.GetLogin();
            var maxProfilesCount = 5;

            var userLogin = Request.GetLogin();
            var userEntities = await _repository.GetUserEntitiesLoginsAsync(userLogin);
            if (userEntities.Count >= 5)
            {
                return BadRequest($"You can create up to {maxProfilesCount} profiles");
            }

            model.UserLogin = login;
            await _updateService.CreateEntity(model);
            return Ok(new BaseResponse() { Success = true });

        }


        /// <summary>
        /// Обновление любых полей профиля (кроме автара)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [Authorize]
        public async Task<IHttpActionResult> Update([ModelBinder(typeof(FunkmapModelBinderProvider))]Profile model)
        {
            await _updateService.UpdateEntity(model);
            return Ok(new BaseResponse() { Success = true });
        }

        /// <summary>
        /// Обновление аватара профиля
        /// </summary>
        /// <param name="request">Логин профиля и фото base64</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateavatar")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateAvatar(UpdateAvatarRequest request)
        {
            //todo валидация

            await _repository.UpdateAvatarAsync(request.Login, request.Photo);

            //_eventBus.PublishAsync()

            return Ok(new BaseResponse() { Success = true });
        }

        /// <summary>
        /// Удаление профиля
        /// </summary>
        /// <param name="login">Логин профиля</param>
        /// <returns></returns>
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

        /// <summary>
        /// Изменение отметки об избранном
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [Route("updateFavorite")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateFavorite(UpdateFavoriteRequest request)
        {
            var login = Request.GetLogin();
            var parameter = new UpdateFavoriteParameter
            {
                EntityLogin = request.EntityLogin,
                IsFavorite = request.IsFavorite,
                UserLogin = login
            };
            await _repository.UpdateFavoriteAsync(parameter);
            return Ok(new BaseResponse() { Success = true });
        }
    }
}
