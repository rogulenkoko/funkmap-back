using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Objects;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Statistics.Data.Repositories
{
    public class CityStatisticsRepository : MongoRepository<CityStatisticsEntity>, IProfileStatisticsRepository
    {
        private readonly IMongoCollection<BaseEntity> _profileCollection;
        private string _map;
        private string _reduce;
        private string _finalize;
        public StatisticsType StatisticsType => StatisticsType.City;
        public CityStatisticsRepository(IMongoCollection<CityStatisticsEntity> collection,
            IMongoCollection<BaseEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
            InitFunction();
        }
        private void InitFunction()
        {
            string other = "\"other\"";
            _map = @"
                function() {
                    var baseEntity = this;
                    var nameCity = 'other';
                    if(Math.abs(baseEntity.loc.coordinates[0]-55.755814)<=0.16&&
                       Math.abs(baseEntity.loc.coordinates[1]-37.617635)<=0.24){
                        nameCity = 'Moscow';
                    }
                    if(Math.abs(baseEntity.loc.coordinates[0]-59.948272)<=0.13&&
                       Math.abs(baseEntity.loc.coordinates[1]-30.317846)<=0.24){
                        nameCity = 'Saint-Petersburg';
                    }
                    if(Math.abs(baseEntity.loc.coordinates[0]-53.894548)<=0.10&&
                       Math.abs(baseEntity.loc.coordinates[1]-30.330654)<=0.13){
                        nameCity = 'Mogilev';
                    }
                    if(Math.abs(baseEntity.loc.coordinates[0]-58.570410)<=0.12&&
                       Math.abs(baseEntity.loc.coordinates[1]-49.559553)<=0.15){
                        nameCity = 'KirovEpta';
                    }
                    if(Math.abs(baseEntity.loc.coordinates[0]-53.902496)<=0.08&&
                       Math.abs(baseEntity.loc.coordinates[1]-27.561481)<=0.21){
                        nameCity = 'Minsk';
                    }
                    emit(nameCity, { count: 1 });
                }";
            _reduce = @"        
                function(key, values) {
                    var sum = 0;
                    for(var i in values) {
		                sum += values[i];
	                }
                    return sum;                    
                }";
            _finalize = @"
                function(key, value){
                   
                }";
        }
        public override async Task UpdateAsync(CityStatisticsEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            entity.LastUpdate = DateTime.UtcNow;
            var result = await _collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);
            if (result == null) await CreateAsync(entity);
        }

        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
            var options = new MapReduceOptions<BaseEntity, CityStatistic>();
            
            var statistics = await _profileCollection.MapReduceAsync(_map, _reduce, options);
            List<CountStatisticsEntity<string>> countStatistics=new List<CountStatisticsEntity<string>>();
            foreach (var itemStatistic in statistics.Current)
            {
                var temp = new CountStatisticsEntity<string>();
                {
                    temp.Key = itemStatistic.NameCity;
                    temp.Count = (int)itemStatistic.Count;
                }
                countStatistics.Add(temp);
            }
            var statistic = new CityStatisticsEntity()
            {
                CountStatistics = countStatistics
            };
            return statistic;
        }

        public async Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            var filter = Builders<BaseEntity>.Filter.Gte(x => x.CreationDate, begin) &
                         Builders<BaseEntity>.Filter.Lte(x => x.CreationDate, end);
            var options = new MapReduceOptions<BaseEntity, CountStatisticsEntity<string>>();
            options.Filter = filter;
            var statistics = _profileCollection.MapReduce(_map, _reduce, options).Current;
            List<CountStatisticsEntity<string>> countStatistics = new List<CountStatisticsEntity<string>>();
            foreach (var itemStatistic in statistics)
            {
                countStatistics.Add(itemStatistic);
            }
            var statistic = new CityStatisticsEntity()
            {
                CountStatistics = countStatistics
            };
            return statistic;
        }

        
    }
}
