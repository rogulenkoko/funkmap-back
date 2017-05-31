using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Module.Musician.Abstract;

namespace Funkmap.Module.Musician.Data
{
    public class MusicianRepository : Repository<MusicianEntity>, IMusicianRepository
    {
        public MusicianRepository(DbContext context) : base(context)
        {
        }
    }
}
