using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Models;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/shop")]
    public class ShopController : ApiController
    {
        private readonly IShopRepository _shopRepository;

        public ShopController(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }
       
        [HttpGet]
        [Route("get/{login}")]
        public async Task<IHttpActionResult> GetShop(string login)
        {
            var shopEntity = await _shopRepository.GetAsync(login);
            var shop = shopEntity.ToPreviewModel();
            return Content(HttpStatusCode.OK, shop);
        }

        [Authorize]
        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> SaveMusician(ShopModel model)
        {
            var entity = model.ToShopEntity();
            var response = new BaseResponse();

            var existingShop = await _shopRepository.GetAsync(model.Login);
            if (existingShop != null)
            {
                return Content(HttpStatusCode.OK, response);
            }

            var userLogin = Request.GetLogin();
            entity.UserLogin = userLogin;

            await _shopRepository.CreateAsync(entity);
            response.Success = true;
            return Content(HttpStatusCode.OK, response);

        }
    }
}