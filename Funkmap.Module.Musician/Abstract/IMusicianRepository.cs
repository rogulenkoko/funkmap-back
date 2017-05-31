using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Abstract.Data;
using Funkmap.Module.Musician.Data;

namespace Funkmap.Module.Musician.Abstract
{
    public interface IMusicianRepository : IRepository<MusicianEntity>
    {
    }
}
