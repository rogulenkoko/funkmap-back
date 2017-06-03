using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Abstract.Search;
using Funkmap.Module.Musician.Mappers;
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
            var musicians = await _musicianRepository.GetAllAsync();
            var searchResults = musicians.Select(x => x.ToSearchModel()).ToList();
            return searchResults;
        }
    }
}
