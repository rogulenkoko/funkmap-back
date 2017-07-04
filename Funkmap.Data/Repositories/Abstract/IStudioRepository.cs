using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Data.Entities;

namespace Funkmap.Data.Repositories.Abstract
{
    public interface IStudioRepository : IMongoRepository<StudioEntity>
    {
    }
}
