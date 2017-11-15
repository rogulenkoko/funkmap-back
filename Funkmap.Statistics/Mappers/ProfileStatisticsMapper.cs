using System.Collections.Generic;
using System.Linq;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Models;

namespace Funkmap.Statistics.Mappers
{
    public static class ProfileStatisticsMapper
    {
        public static ICollection<TopProfileStatistics> ToModel(this TopEntityStatisticsEntity source)
        {
            if (source == null || source.CountStatistics == null) return null;
            return source.CountStatistics.Select(x=> new TopProfileStatistics()
            {
                Count = x.Count,
                Login = x.Login,
                EntityType = x.EntityType,
                
            }).ToList();
        }

        public static ICollection<ProfileTypeStatistics> ToModel(this EntityTypeStatisticsEntity source)
        {
            if (source == null || source.CountStatistics == null) return null;
            return source.CountStatistics.Select(x => new ProfileTypeStatistics()
            {
                Count = x.Count,
                EntityType = x.Key

            }).ToList();
        }
    }
}
