using System.Collections.Generic;
using System.Linq;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Models;
    
namespace Funkmap.Statistics.Mappers
{
    public static class MusicianStatisticsMapper
    {
        public static ICollection<StyleStatistics> ToModel(this TopStylesStatisticsEntity source)
        {
            if (source?.CountStatistics == null) return null;
            return source.CountStatistics.Select(x=> new StyleStatistics()
            {
                Style = x.Key,
                Count = x.Count
            }).ToList();
        }
        

        public static ICollection<SexStatistics> ToModel(this SexStatisticsEntity source)
        {
            if (source?.CountStatistics == null) return null;
            return source.CountStatistics.Select(x => new SexStatistics()
            {
                Sex = x.Key,
                Count = x.Count
            }).ToList();
        }

        public static ICollection<InstrumentStatistics> ToModel(this InstrumentStatisticsEntity source)
        {
            if (source?.CountStatistics == null) return null;
            return source.CountStatistics.Select(x => new InstrumentStatistics()
            {
                Instrument = x.Key,
                Count = x.Count
            }).ToList();
        }
    }
}
