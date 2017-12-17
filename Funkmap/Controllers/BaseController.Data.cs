using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Mappers;

namespace Funkmap.Controllers
{
    public partial class BaseController
    {

        /// <summary>
        /// Все активные профили
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _repository.GetAllAsyns();
            var markers = result.Select(x => x.ToMarkerModel()).ToList();
            return Content(HttpStatusCode.OK, markers);
        }

        /// <summary>
        /// Полная информация о профиле
        /// </summary>
        /// <param name="login">Логин профиля</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getFull/{id}")]
        public async Task<IHttpActionResult> GetFullBand(string login)
        {
            var entity = await _repository.GetAsync(login);
            return Content(HttpStatusCode.OK, entity.ToSpecificModel());

        }

        /// <summary>
        /// Основная информация о профиле (специфична для каждого типа профиля)
        /// </summary>
        /// <param name="login">Логин профиля</param>
        /// <returns></returns>

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetRehearsalPoint(string login)
        {
            var entity = await _repository.GetAsync(login);
            return Content(HttpStatusCode.OK, entity.ToSpecificPreviewModel());
        }

        /// <summary>
        /// Основная информация о некоторых профилях
        /// </summary>
        /// <param name="logins">Логины профилей</param>
        /// <returns></returns>
        [HttpPost]
        [Route("specific")]
        public async Task<IHttpActionResult> GetSpecific(string[] logins)
        {
            var baseEntities = await _repository.GetSpecificFullAsync(logins);
            var items = baseEntities.Select(x => x.ToSearchModel());
            return Ok(items);
        }

        /// <summary>
        /// Аватар профиля
        /// </summary>
        /// <param name="login">Логин профиля</param>
        /// <returns></returns>
        [HttpPost]
        [Route("getimage")]
        public async Task<IHttpActionResult> GetImage(string login)
        {
            var file = await _repository.GetFileAsync(login);
            return Ok(file);
        }

        /// <summary>
        /// Логины профилей, созданных пользователем
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("users")]
        public async Task<IHttpActionResult> GetUserEntitiesLogins()
        {
            var userLogin = Request.GetLogin();
            var logins = await _repository.GetUserEntitiesLoginsAsync(userLogin);
            return Ok(logins);
        }

        /// <summary>
        /// Соотношение типов профилей и количества созданных пользователем профилей данного типа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("userscount")]
        public async Task<IHttpActionResult> GetUserEntitiesCountInfo()
        {
            var userLogin = Request.GetLogin();
            var countResults = await _repository.GetUserEntitiesCountInfoAsync(userLogin);
            var result = countResults.ToCountModels();
            return Ok(result);
        }

        /// <summary>
        /// Проверка на существование профиля
        /// </summary>
        /// <param name="login">Логин профиля</param>
        /// <returns></returns>
        [HttpGet]
        [Route("checkLogin/{login}")]
        public async Task<IHttpActionResult> CheckIfLoginExist(string login)
        {
            var isExist = await _repository.CheckIfLoginExistAsync(login);
            return Ok(isExist);
        }

        /// <summary>
        /// Отмеченные, как избранное, логины профилей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("favoritesLogins")]
        public async Task<IHttpActionResult> GetFavoritesLogins()
        {
            var login = Request.GetLogin();
            var favoritesLogins = await _repository.GetFavoritesLoginsAsync(login);
            return Ok(favoritesLogins);
        }


        /// <summary>
        /// Отмеченные, как избранное, профиля
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("favorites")]
        public async Task<IHttpActionResult> GetFavorites()
        {
            var login = Request.GetLogin();
            var favoritesLogins = await _repository.GetFavoritesLoginsAsync(login);
            var favorites = await _repository.GetSpecificFullAsync(favoritesLogins.ToArray());
            return Ok(favorites?.Select(x=>x.ToSearchModel()));
        }
    }
}
