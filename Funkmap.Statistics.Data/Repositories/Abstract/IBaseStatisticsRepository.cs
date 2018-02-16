using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Statistics.Data.Entities;

namespace Funkmap.Statistics.Data.Repositories.Abstract
{
    public interface IBaseStatisticsRepository
    {
        Task<ICollection<BaseStatisticsEntity>> GetAllStatisticsAsync(IReadOnlyCollection<StatisticsType> statisticsTypes = null);
    }
}
