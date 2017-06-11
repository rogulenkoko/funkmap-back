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
    public class MusicianSearchService : ISearchService
    {
        private readonly IMusicianRepository _musicianRepository;

        public MusicianSearchService(IMusicianRepository musicianRepository)
        {
            _musicianRepository = musicianRepository;
        }

        public async Task<ICollection<SearchModel>> SearchAllAsync()

        {
            var musicians = await _musicianRepository.GetMusicianPreviews();
            var searchResults = musicians.Select(x => x.ToSearchModel()).ToList();
            return searchResults;
        }

        public async Task<ICollection<SearchModel>> SearchNearest(SearchRequest request)
        {
            var searchParametr = new LocationParameter()
            {
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                RadiusDeg = request.RadiusDeg
            };
            var musicians = await _musicianRepository.GetNearestMusicianPreviews(searchParametr);

            var musiciansResult = musicians.Select(x => x.ToSearchModel()).ToList();

            return musiciansResult;
        }
    }
}
