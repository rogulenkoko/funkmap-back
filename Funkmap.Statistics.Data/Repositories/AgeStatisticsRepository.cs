using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Objects;
using Funkmap.Statistics.Data.Repositories.Abstract;
using Funkmap.Statistics.Data.Services;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class AgeStatisticsRepository: StatisticsMongoRepository<AgeStatisticsEntity>, IMusicianStatisticsRepository
    {
        public StatisticsType StatisticsType => StatisticsType.Age;

        private readonly IMongoCollection<MusicianEntity> _profileCollection;

        private readonly IAgeInfoProvider _ageInfoProvider;

        public AgeStatisticsRepository(IMongoCollection<AgeStatisticsEntity> collection,
            IMongoCollection<MusicianEntity> profileCollection,
            IAgeInfoProvider ageInfoProvider) : base(collection)
        {
            _ageInfoProvider = ageInfoProvider;
            _profileCollection = profileCollection;
        }

        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
            /*var mapFunc = function(){
                if(this._t[1]!='MusicianEntity'){
                    return;}
                if(this.bd==null){
                    emit('other',this.log)
                    }
                var currentDate = new Date();
                var timeDiff = Math.abs(currentDate.getTime() - this.bd);
                var year = Math.ceil(timeDiff / (1000 * 3600 * 24 * 365));
                var ages = [{ start: 0,finish: 10,des: "0 - 10"},
                { start: 10,finish: 19,des: "10 - 19"},
                { start: 19,finish: 28,des: "19 - 28"},
                { start: 28,finish: 37,des: "28 - 37"},
                { start: 37,finish: 46,des: "37 - 46"},
                { start: 46,finish: 100,des: "46 - 100"}];
                for (var i = 0; i < ages.length; i++)
                {
                    if ((ages[i].start <= year) && (ages[i].finish > year))
                    {
                        emit(ages[i].des, this.log);
                    }
                }
            }
            var reduceFunc = function(key, values){ var statistics = values.length; return statistics; }
            var finalizeFunc = function(key, reduced){ if (typeof reduced === 'string') return 1; return reduced; }

            db.bases.mapReduce(mapFunc, reduceFunc,{ finalize: finalizeFunc,out:{ inline: 1} })*/

            var mapFunc = GetMapFunction();
            var reduceFunc = GetReduseFunction();
            var finalizeFunc = GetFinalizeFunction();

            var options = new MapReduceOptions<MusicianEntity, AgeStatistic>()
            {
                Finalize = finalizeFunc
            };

            var statistics = await _profileCollection.MapReduce(mapFunc, reduceFunc, options).ToListAsync();

            var countStatistics = statistics.Select(x => new CountStatisticsEntity<string>()
            {
                Count = x.Count,
                Key = x.Desc
            }).ToList();
            return new AgeStatisticsEntity() { CountStatistics = countStatistics };
        }

        public Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            //не уместна фильтрация по дате
            return BuildFullStatisticsAsync();
        }

        
        private string GetMapFunction(DateTime? begin = null, DateTime? end = null)
        {
            var sb = new StringBuilder();
            var ages = _ageInfoProvider.AgeInfos;
            sb.Append("if(this._t[1]!='MusicianEntity'){return;}");
            sb.Append(" if(this.bd==null){"+
           "emit('other', this.log)}");
            sb.Append("var currentDate = new Date();");
            sb.Append("var timeDiff = Math.abs(currentDate.getTime() - this.bd);"+
                      "var year = Math.ceil(timeDiff / (1000 * 3600 * 24 * 365));");
            sb.Append("var ages = [");
            foreach (var age in ages)
            {
                sb.Append($"{{start:{age.Start},finish:{age.Finish},des:'{age.AgePeriod}'}},");
                //sb.Append($"{{name:\"{city.Name}\", center: {{lat: {city.CenterLatitude}, lon: {city.CenterLongitude}}}, radius:{city.Radius}}},");
            }
            sb.Append("];");

            if (!begin.HasValue)
            {
                sb.Append("for(var i = 0; i < ages.length; i++)" +
                          "{" +
                            "if((ages[i].start<=year)&&(ages[i].finish>year))" +
                            "{" +
                                "emit(ages[i].des, this.log);" +
                            "}" +
                          "}");
            }
            else
            {
                if (!end.HasValue) end = DateTime.UtcNow;
                sb.Append($"var begin = new Date({begin.Value.Year}, {begin.Value.Month}, {begin.Value.Day}); var end = new Date({end.Value.Year}, {end.Value.Month}, {end.Value.Day}); for(var i = 0; i < ages.length; i++){{if((ages[i].start<=year)&&(ages[i].finish>year)){{if(this.cd >= begin && this.cd <= end){{emit(ages[i].des, this.log);}}}}}}");
            }



            return WrapWithJsFunction(sb.ToString());
        }

        private string GetReduseFunction()
        {
            var functionBody = "var statistics = values.length;return statistics;";
            return WrapWithJsFunction(functionBody, new[] { "key", "values" });
        }

        private string GetFinalizeFunction()
        {
            var functionBody = "if(typeof reduced === \'string\') return 1;return reduced;";
            return WrapWithJsFunction(functionBody, new[] { "key", "reduced" });
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
