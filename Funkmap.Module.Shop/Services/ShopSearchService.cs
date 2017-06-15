using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Abstract.Search;
using Funkmap.Common.Data.Parameters;
using Funkmap.Module.Search.Models;
using Funkmap.Module.Shop.Mappers;
using Funkmap.Shop.Data.Abstract;

namespace Funkmap.Module.Shop.Services
{
    public class ShopSearchService : ISearchService
    {
        private readonly IShopRepository _shopRepository;
        public ShopSearchService(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }
        public async Task<ICollection<SearchModel>> SearchAllAsync()
        {
            var shops = await _shopRepository.GetShopsPreviews();
            var searchResults = shops.Select(x => x.ToSearchModel()).ToList();
            return searchResults;
        }

        public async Task<ICollection<SearchModel>> SearchNearest(NearestRequest request)
        {
            var searchParametr = new LocationParameter()
            {
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                RadiusDeg = request.RadiusDeg
            };
            var shops = await _shopRepository.GetNearestShopsPreviews(searchParametr);
            var searchResults = shops.Select(x => x.ToSearchModel()).ToList();
            return searchResults;
        }

        /*public async Task<ICollection<SearchModel>> SearchByName(string storeName)
        {
            var shops = await _shopRepository.GetShopsPreviewsSearchByName(storeName);
            return shops.Select(x => x.ToSearchModel()).ToList();
        }*/
    }
}
