using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Objects;
using Funkmap.Statistics.Data.Repositories.Abstract;
using Funkmap.Statistics.Data.Services;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class CityStatisticsRepository : StatisticsRepository<CityStatisticsEntity>, IProfileStatisticsRepository
    {
        private readonly IMongoCollection<BaseEntity> _profileCollection;
        private readonly ICitiesInfoProvider _citiesInfoProvider;

        public StatisticsType StatisticsType => StatisticsType.City;

        public CityStatisticsRepository(IMongoCollection<CityStatisticsEntity> collection,
            IMongoCollection<BaseEntity> profileCollection,
            ICitiesInfoProvider citiesInfoProvider) : base(collection)
        {
            _profileCollection = profileCollection;
            _citiesInfoProvider = citiesInfoProvider;
        }

        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {

            //var mapFunc = function(){

            //    var cities = [
            //    { name: "Санкт-Петербург", center: { lat: 50, lon: 30}, radius: 2 },
            //    { name: "Минск", center: { lat: 55, lon: 29}, radius: 2 }
            //    ]

            //    for (var i = 0; i < cities.length; i++)
            //    {
            //        if ((cities[i].center.lon - cities[i].radius <= this.loc.coordinates[0] && this.loc.coordinates[0] <= cities[i].center.lon + cities[i].radius)
            //            && (cities[i].center.lat - cities[i].radius <= this.loc.coordinates[1] && this.loc.coordinates[1] <= cities[i].center.lat + cities[i].radius))
            //        {
            //            emit(cities[i].name, this.log);
            //        }
            //    }
            //}

            //var reduceFunc = function(key, values){
            //    var statistics = values.length;
            //    return statistics;
            //}

            //var finalizeFunc = function(key, reduced){
            //    if (typeof reduced === 'string') return 1;
            //    return reduced;
            //}

            //db.bases.mapReduce(
            //    mapFunc,
            //    reduceFunc,
            //{
            //    finalize: finalizeFunc,
            //        out: { inline: 1 }
            //}
            //)

            var mapFunc = GetMapFunction();
            var reduceFunc = GetReduseFunction();
            var finalizeFunc = GetFinalizeFunction();

            var options = new MapReduceOptions<BaseEntity, CityStatistic>()
            {
                Finalize = finalizeFunc
            };

            var statistics = await _profileCollection.MapReduce(mapFunc, reduceFunc, options).ToListAsync();

            var countStatistics = statistics.Select(x => new CountStatisticsEntity<string>()
            {
                Count = x.Count,
                Key = x.City
            }).ToList();

            return new CityStatisticsEntity() {CountStatistics = countStatistics };
        }

        public async Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            var mapFunc = GetMapFunction(begin, end);
            var reduceFunc = GetReduseFunction();
            var finalizeFunc = GetFinalizeFunction();

            var options = new MapReduceOptions<BaseEntity, CityStatistic>()
            {
                Finalize = finalizeFunc
            };

            var statistics = await _profileCollection.MapReduce(mapFunc, reduceFunc, options).ToListAsync();

            var countStatistics = statistics.Select(x => new CountStatisticsEntity<string>()
            {
                Count = x.Count,
                Key = x.City
            }).ToList();

            return new CityStatisticsEntity() { CountStatistics = countStatistics };
        }

        private string GetMapFunction(DateTime? begin = null, DateTime? end = null)
        {
            var sb = new StringBuilder();
            var cities = _citiesInfoProvider.CityInfos;
            sb.Append("var cities = [");
            foreach (var city in cities)
            {
                sb.Append($"{{name:\"{city.Name}\", center: {{lat: {city.CenterLatitude}, lon: {city.CenterLongitude}}}, radius:{city.Radius}}},");
            }
            sb.Append("];");

            if (!begin.HasValue)
            {
                sb.Append("for(var i = 0; i < cities.length; i++){if((cities[i].center.lon - cities[i].radius <= this.loc.coordinates[0] && this.loc.coordinates[0] <= cities[i].center.lon + cities[i].radius)&& (cities[i].center.lat - cities[i].radius <= this.loc.coordinates[1] && this.loc.coordinates[1] <= cities[i].center.lat + cities[i].radius)){emit(cities[i].name, this.log);}}");
            }
            else
            {
                if(!end.HasValue) end = DateTime.UtcNow;
                sb.Append($"var begin = new Date({begin.Value.Year}, {begin.Value.Month - 1}, {begin.Value.Day}, {begin.Value.Hour}, {begin.Value.Minute}, {begin.Value.Second}); var end = new Date({end.Value.Year}, {end.Value.Month - 1}, {end.Value.Day}, {end.Value.Hour}, {end.Value.Minute}, {end.Value.Second}); for(var i = 0; i < cities.length; i++){{if((cities[i].center.lon - cities[i].radius <= this.loc.coordinates[0] && this.loc.coordinates[0] <= cities[i].center.lon + cities[i].radius)&& (cities[i].center.lat - cities[i].radius <= this.loc.coordinates[1] && this.loc.coordinates[1] <= cities[i].center.lat + cities[i].radius)){{if(this.cd >= begin && this.cd <= end){{emit(cities[i].name, this.log);}}}}}}");
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
