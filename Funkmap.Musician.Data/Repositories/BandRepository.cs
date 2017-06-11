using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Data;
using Funkmap.Common.Data.Parameters;
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

        public async Task<ICollection<BandEntity>> GetNearestBandsPreviews(LocationParameter locationParameter)
        {
            var query = Context.Set<BandEntity>().Select(x => x);
            if (locationParameter != null)
            {
                query = query.Where(x => x.Longitude <= locationParameter.Longitude + locationParameter.RadiusDeg
                                      && x.Longitude >= locationParameter.Longitude - locationParameter.RadiusDeg
                                      && x.Latitude <= locationParameter.Latitude + locationParameter.RadiusDeg
                                      && x.Latitude >= locationParameter.Latitude - locationParameter.RadiusDeg);
            }

            var bandPreviews = await SelectPreviews(query);

            return bandPreviews;
        }

        private async Task<ICollection<BandEntity>> SelectPreviews(IQueryable<BandEntity> query)
        {
            var bands = await query.Select(x => new
            {
                Latitude = x.Latitude,
                Login = x.Login,
                Longitude = x.Longitude,
                Id = x.Id
            }).ToListAsync();

            return bands.Select(x => new BandEntity()
            {
                Latitude = x.Latitude,
                Login = x.Login,
                Longitude = x.Longitude,
                Id = x.Id,
            }).ToList();
        }
    }
}
