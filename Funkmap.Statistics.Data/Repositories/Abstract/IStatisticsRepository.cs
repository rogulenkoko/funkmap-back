using System;
using System.Threading.Tasks;
using Funkmap.Statistics.Data.Entities;

namespace Funkmap.Statistics.Data.Repositories.Abstract
{
    public interface IStatisticsRepository
    {
        Task<BaseStatisticsEntity> BuildFullStatisticsAsync();

        Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end);

        StatisticsType StatisticsType { get; }
    }
}
