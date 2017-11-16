using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Data.Entities;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Objects;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class InBandStatisticsRepository : StatisticsMongoRepository<InBandStatisticsEntity>, IMusicianStatisticsRepository
    {
        public StatisticsType StatisticsType => StatisticsType.InBand;

        private readonly IMongoCollection<MusicianEntity> _profileCollection;

        public InBandStatisticsRepository(IMongoCollection<InBandStatisticsEntity> collection, IMongoCollection<MusicianEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
        }

        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {

            //var mapFunc = function(){
            //    if (this.band == null || this.band.length == 0) emit(false, this.log);
            //    else emit(true, this.log)
            //}

            //var reduceFunc = function(key, values){
            //    var statistics = values.length;
            //    return statistics;
            //}

            //db.bases.mapReduce(
            //    mapFunc,
            //    reduceFunc,
            //{
            //    out: { inline: 1 }
            //}
            //)

            var mapFunc = GetMapFunction();
            var reduceFunc = GetReduseFunction();

            var options = new MapReduceOptions<MusicianEntity, InBandStatistics>();

            var statistics = await _profileCollection.MapReduce(mapFunc, reduceFunc, options).ToListAsync();

            var countStatistics = statistics.Select(x => new CountStatisticsEntity<bool>()
                {
                    Count = x.Count,
                    Key = x.IsInBand
                })
                .ToList();

            return new InBandStatisticsEntity()
            {
                CountStatistics = countStatistics
            };
            

        }

        public async Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            var mapFunc = GetMapFunction(begin, end);
            var reduceFunc = GetReduseFunction();
            var options = new MapReduceOptions<MusicianEntity, InBandStatistics>();

            var statistics = await _profileCollection.MapReduce(mapFunc, reduceFunc, options).ToListAsync();

            var countStatistics = statistics.Select(x => new CountStatisticsEntity<bool>()
                {
                    Count = x.Count,
                    Key = x.IsInBand
                })
                .ToList();

            return new InBandStatisticsEntity()
            {
                CountStatistics = countStatistics
            };
        }


        private string GetMapFunction(DateTime? begin = null, DateTime? end = null)
        {
            string functionBody;

            if (begin.HasValue)
            {
                if(!end.HasValue) end = DateTime.UtcNow;
                functionBody = $"var begin = new Date({begin.Value.Year}, {begin.Value.Month - 1}, {begin.Value.Day}, {begin.Value.Hour}, {begin.Value.Minute}, {begin.Value.Second}); var end = new Date({end.Value.Year}, {end.Value.Month - 1}, {end.Value.Day}, {end.Value.Hour}, {end.Value.Minute}, {end.Value.Second});if(this.cd < begin || this.cd > end) return;if(this.band == null || this.band.length == 0) emit(false, this.log);else emit(true, this.log)";
            }
            else
            {
                functionBody = "if(this.band == null || this.band.length == 0) emit(false, this.log); else emit(true, this.log);";
            }
            
            return WrapWithJsFunction(functionBody);
        }

        private string GetReduseFunction()
        {
            var functionBody = "var statistics = values.length;return statistics;";
            return WrapWithJsFunction(functionBody, new[] { "key", "values" });
        }

        private string WrapWithJsFunction(string body, string[] parameters = null)
        {

            var parametersString = String.Empty;

            if (parameters != null)
            {
                parametersString = String.Join(",", parameters);
            }

            return $"function({parametersString}){{{body}}}";
        }
    }
}
