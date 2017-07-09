using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Parameters;
using MongoDB.Driver;

namespace Funkmap.Data.Services.Abstract
{
    public interface IFilterFactory
    {
        FilterDefinition<BaseEntity> CreateFilter(IFilterParameter parameter);
    }
}
