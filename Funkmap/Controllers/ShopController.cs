using System.Net;
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
    [RoutePrefix("api/shop")]
    public class ShopController : ApiController
    {
        private readonly IShopRepository _shopRepository;

        public ShopController(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        [HttpGet]
        [Route("getFull/{id}")]
        public async Task<IHttpActionResult> GetFullShop(string id)
        {
            var shopEntity = await _shopRepository.GetAsync(id);
            ShopModel shop = shopEntity.ToModel();
            return Content(HttpStatusCode.OK, shop);

        }

        [HttpGet]
        [Route("get/{login}")]
        public async Task<IHttpActionResult> GetShop(string login)
        {
            var shopEntity = await _shopRepository.GetAsync(login);
            var shop = shopEntity.ToPreviewModel();
            return Content(HttpStatusCode.OK, shop);
        }
    }
}