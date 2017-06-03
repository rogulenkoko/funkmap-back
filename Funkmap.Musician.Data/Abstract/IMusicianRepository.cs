using System.Collections.Generic;
using Funkmap.Common.Data.Abstract;
using Funkmap.Musician.Data.Entities;
using Funkmap.Musician.Data.Parameters;

namespace Funkmap.Musician.Data.Abstract
{
    public interface IMusicianRepository : IRepository<MusicianEntity>
    {
        ICollection<MusicianEntity> GetFiltered(MusicianParameter parameter);
    }
}
