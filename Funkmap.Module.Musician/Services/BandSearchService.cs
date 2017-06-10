using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Abstract.Search;
using Funkmap.Module.Musician.Mappers;
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
    }
}
