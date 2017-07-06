using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;

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
    }
}