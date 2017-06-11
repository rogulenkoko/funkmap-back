using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Data;
using Funkmap.Common.Data.Parameters;
using Funkmap.Musician.Data.Abstract;
using Funkmap.Musician.Data.Entities;
using Funkmap.Musician.Data.Parameters;

namespace Funkmap.Musician.Data.Repositories
{
    public class MusicianRepository : Repository<MusicianEntity>, IMusicianRepository
    {
        public MusicianRepository(MusicianContext context) : base(context)
        {
        }

        public async Task<ICollection<MusicianEntity>> GetMusicianPreviews()
        {
            var query = Context.Set<MusicianEntity>().Select(x=>x);
            var musicianPreviews = await SelectPreviews(query);
            return musicianPreviews;
        }

        public async Task<ICollection<MusicianEntity>> GetFiltered(MusicianParameter parameter)
        {
            Styles styleFilter = parameter.Styles.FirstOrDefault();
            if (parameter.Styles.Count > 1)
            {
                for (int i = 1; i < parameter.Styles.Count; i++)
                {
                    styleFilter = styleFilter | parameter.Styles.ElementAt(i);
                }
            }
            var musicianEntities = await Context.Set<MusicianEntity>().Where(x => x.Styles.HasFlag(styleFilter)).ToListAsync();
            return musicianEntities;
        }

        public async Task<ICollection<MusicianEntity>> GetNearestMusicianPreviews(LocationParameter locationParameter)
        {
            var query = Context.Set<MusicianEntity>().Select(x => x);
            if (locationParameter != null)
            {
                query = query.Where(x => x.Longitude <= locationParameter.Longitude + locationParameter.RadiusDeg
                                         && x.Longitude >= locationParameter.Longitude - locationParameter.RadiusDeg
                                         && x.Latitude <= locationParameter.Latitude + locationParameter.RadiusDeg
                                         && x.Latitude >= locationParameter.Latitude - locationParameter.RadiusDeg);
            }


            var musicianPreviews = await SelectPreviews(query);
            return musicianPreviews;
        }

        private async Task<ICollection<MusicianEntity>> SelectPreviews(IQueryable<MusicianEntity> query)
        {
            var musicianPreviewsObj = await query.Select(x => new
            {
                Latitude = x.Latitude,
                Login = x.Login,
                Longitude = x.Longitude,
                Id = x.Id,
                Instrument = x.Instrument
            }).ToListAsync();

            var musicianPreviews = musicianPreviewsObj.Select(x => new MusicianEntity()
            {
                Latitude = x.Latitude,
                Login = x.Login,
                Longitude = x.Longitude,
                Id = x.Id,
                Instrument = x.Instrument
            }).ToList();
            return musicianPreviews;
        }
    }
}
