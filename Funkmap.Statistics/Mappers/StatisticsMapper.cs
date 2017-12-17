using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Models;

namespace Funkmap.Statistics.Mappers
{
    public static class StatisticsMapper
    {
        public static ICollection<IStatistics> ToStatisticsModel(this BaseStatisticsEntity entity, StatisticsType type)
        {
            if (entity == null) return null;

            switch (type)
            {
                case StatisticsType.Age: return (entity as AgeStatisticsEntity).ToModel().Select(x => x as IStatistics).ToList();
                case StatisticsType.EntityType: return (entity as EntityTypeStatisticsEntity).ToModel().Select(x => x as IStatistics).ToList();
                case StatisticsType.City: return (entity as CityStatisticsEntity).ToModel().Select(x => x as IStatistics).ToList();
                case StatisticsType.InBand: return (entity as InBandStatisticsEntity).ToModel().Select(x => x as IStatistics).ToList();
                case StatisticsType.InstrumentType: return (entity as InstrumentStatisticsEntity).ToModel().Select(x => x as IStatistics).ToList();
                case StatisticsType.SexType: return (entity as SexStatisticsEntity).ToModel().Select(x => x as IStatistics).ToList();
                case StatisticsType.TopEntity: return (entity as TopProfileStatisticsEntity).ToModel().Select(x => x as IStatistics).ToList();
                case StatisticsType.TopStyles: return (entity as TopStylesStatisticsEntity).ToModel().Select(x => x as IStatistics).ToList();

                default: throw new InvalidOperationException("Некорректный тип статистик");
            }
        }
    }
}
