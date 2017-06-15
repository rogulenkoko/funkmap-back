using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Module.Shop.Mappers;
using Funkmap.Shop.Data.Abstract;

namespace Funkmap.Module.Shop.Controllers
{
    [RoutePrefix("api/shop")]
    public class ShopControllers : ApiController
    {
        private readonly IShopRepository _shopRepository;

        public ShopControllers(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetShops()
        {
            var allShops = await _shopRepository.GetAllAsync();
            return Content(HttpStatusCode.OK, allShops);
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetShop(long id)
        {
            var shopEntity = await _shopRepository.GetAsync(id);
            var shop = shopEntity.ToModel();
            return Content(HttpStatusCode.OK, shop);
        }
        /*
        [HttpGet]
        [Route("getbyname/{name}")]
        public async Task<IHttpActionResult> GetShopsByName(string name)
        {
            var shops = await _shopRepository.GetShopsPreviewsSearchByName(name);
            return Content(HttpStatusCode.OK, shops);

        }*/
    }
}
