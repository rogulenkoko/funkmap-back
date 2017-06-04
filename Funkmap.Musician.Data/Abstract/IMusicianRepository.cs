using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Data.Abstract;
using Funkmap.Musician.Data.Entities;
using Funkmap.Musician.Data.Parameters;

namespace Funkmap.Musician.Data.Abstract
{
    public interface IMusicianRepository : IRepository<MusicianEntity>
    {
        Task<ICollection<MusicianEntity>> GetMusicianPreviews();
        Task<ICollection<MusicianEntity>> GetFiltered(MusicianParameter parameter);
    }
}
