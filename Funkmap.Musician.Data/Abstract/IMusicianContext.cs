using System.Data.Entity;
using Funkmap.Musician.Data.Entities;

namespace Funkmap.Musician.Data.Abstract
{
    public interface IMusicianContext
    {
        DbSet<MusicianEntity> Musicians { get; set; }
    }
}
