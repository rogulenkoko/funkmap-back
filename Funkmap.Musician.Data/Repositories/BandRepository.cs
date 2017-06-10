using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Data;
using Funkmap.Musician.Data.Abstract;
using Funkmap.Musician.Data.Entities;

namespace Funkmap.Musician.Data.Repositories
{
    public class BandRepository : Repository<BandEntity>, IBandRepository
    {
        public BandRepository(MusicianContext context) : base(context)
        {
        }


        public async Task<ICollection<BandEntity>> GetBandsPreviews()
        {
            var bandPreviewsObj = await Context.Set<BandEntity>().Select(x => new
            {
                Latitude = x.Latitude,
                Login = x.Login,
                Longitude = x.Longitude,
                Id = x.Id
            }).ToListAsync();

            var bandPreviews = bandPreviewsObj.Select(x => new BandEntity()
            {
                Latitude = x.Latitude,
                Login = x.Login,
                Longitude = x.Longitude,
                Id = x.Id,
            }).ToList();

            return bandPreviews;
        }
    }
}
