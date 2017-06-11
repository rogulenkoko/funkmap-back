using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Abstract.Search;
using Funkmap.Common.Data.Parameters;
using Funkmap.Module.Musician.Mappers;
using Funkmap.Module.Search.Models;
using Funkmap.Musician.Data.Abstract;

namespace Funkmap.Module.Musician.Services
{
    public class BandSearchService : ISearchService
    {
        private readonly IBandRepository _bandRepository;

        public BandSearchService(IBandRepository bandRepository)
        {
            _bandRepository = bandRepository;
        }

        public async Task<ICollection<SearchModel>> SearchAllAsync()
        {
            var bands = await _bandRepository.GetBandsPreviews();
            var searchResults = bands.Select(x => x.ToSearchModel()).ToList();
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
            var bands = await _bandRepository.GetNearestBandsPreviews(searchParametr);

            var bandsResult = bands.Select(x => x.ToSearchModel()).ToList();

            return bandsResult;
        }
    }
}
