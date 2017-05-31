using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Module.Musician.Data;

namespace Funkmap.Module.Musician.Abstract
{
    public interface IMusicianContext
    {
        DbSet<MusicianEntity> Musicians { get; set; }
    }
}
