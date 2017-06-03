using System.Collections.Generic;
using System.Linq;
using Funkmap.Common;
using Funkmap.Common.Data;
using Funkmap.Musician.Data.Abstract;
using Funkmap.Musician.Data.Entities;
using Funkmap.Musician.Data.Parameters;

namespace Funkmap.Musician.Data
{
    public class MusicianRepository : Repository<MusicianEntity>, IMusicianRepository
    {
        public MusicianRepository(MusicianContext context) : base(context)
        {
        }

        public ICollection<MusicianEntity> GetFiltered(MusicianParameter parameter)
        {
            Styles styleFilter = parameter.Styles.FirstOrDefault();
            if (parameter.Styles.Count > 1)
            {
                for (int i = 1; i < parameter.Styles.Count; i++)
                {
                    styleFilter = styleFilter | parameter.Styles.ElementAt(i);
                }

            }
            var musicianEntities = Context.Set<MusicianEntity>().Where(x => x.Styles.HasFlag(styleFilter)).ToList();//(x.Styles & styleFilter) != 0
            return musicianEntities;
        }
    }
}
