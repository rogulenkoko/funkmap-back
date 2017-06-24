using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Data.Entities;
using Funkmap.Data.Parameters;

namespace Funkmap.Data.Repositories.Abstract
{
    public interface IMusicianRepository : IMongoRepository<MusicianEntity>
    {
        Task<ICollection<MusicianEntity>> GetFilteredMusicians(MusicianFilterParameter parameter);
    }
}
