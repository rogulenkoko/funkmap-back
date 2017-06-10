using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Data;
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
            var musicianPreviewsObj = await Context.Set<MusicianEntity>().Select(x => new 
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
    }
}
