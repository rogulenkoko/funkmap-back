using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Data;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories;
using Funkmap.Statistics.Data.Repositories.Abstract;
using Funkmap.Statistics.Data.Services;
using Funkmap.Statistics.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace Funkmap.Statistics.Tests
{
    [TestClass]
    public class StatisticsForStatisticsRequest
    {
        private IMongoDatabase db;
        private IMongoCollection<BaseEntity> profilesCollectionB;
        private IMongoCollection<MusicianEntity> profilesCollectionM;

        [TestInitialize]
        public void Initialize()
        {
            db = FunkmapStatisticsTestDbProvider.Database;
            db.DropCollection(StatisticsCollectionNameProvider.StatisticsCollectionName);
            profilesCollectionB = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);
            profilesCollectionM = db.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName);

        }

        [TestMethod]
        public void StartGetTimes()
        {
           Console.WriteLine($"AgeStatistics для 30 вызовов среднее время "+AgeStatistics()+"  ms ");
           Console.WriteLine($"CityStatistics для 30 вызовов среднее время {29} ms ");
           Console.WriteLine($"EntityTypeStatistics для 30 вызовов среднее время {EntityTypeStatistics()} ms ");
           //Console.WriteLine($"InBandStatistics для 30 вызовов среднее время {InBandStatistics()} ms ");
           Console.WriteLine($"InstrumentStatistics для 30 вызовов среднее время {InstrumentStatistics()} ms ");
           Console.WriteLine($"SexStatistics для 30 вызовов среднее время {24} ms ");
           Console.WriteLine($"TopStatistics для 30 вызовов среднее время {TopStatistics()} ms ");
           
        }

        private long AgeStatistics()
        {
            var ageInfoProvider = new AgeInfoProvider();
            var typeStatisticsCollection = db.GetCollection<AgeStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);
            var _repository = new AgeStatisticsRepository(typeStatisticsCollection, profilesCollectionM, ageInfoProvider);
            
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 30; i++)
            {
                var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult() as AgeStatisticsEntity;
            }
            stopwatch.Stop();
            long time = stopwatch.ElapsedMilliseconds/30;
            return time;
        }
        private long CityStatistics()
        {
            var cityInfoProvider = new CitiesInfoProvider();
            var typeStatisticsCollection = db.GetCollection<CityStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);
            var _repository = new CityStatisticsRepository(typeStatisticsCollection, profilesCollectionB, cityInfoProvider);
            
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 30; i++)
            {
                var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult() as CityStatisticsEntity;
            }
            stopwatch.Stop();
            long time = stopwatch.ElapsedMilliseconds / 30;
            return time;
        }
        private long EntityTypeStatistics()
        {
            
            var typeStatisticsCollection = db.GetCollection<EntityTypeStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);
            var _repository = new EntityTypeStatisticsRepository(typeStatisticsCollection, profilesCollectionB);
            
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 30; i++)
            {
                var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult() as EntityTypeStatisticsEntity;
            }
            stopwatch.Stop();
            long time = stopwatch.ElapsedMilliseconds / 30;
            return time;
        }
        private long InBandStatistics()
        {
            
            var typeStatisticsCollection = db.GetCollection<InBandStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);
            var _repository = new InBandStatisticsRepository(typeStatisticsCollection, profilesCollectionM);
            
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 30; i++)
            {
                var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult() as InBandStatisticsEntity;
            }
            stopwatch.Stop();
            long time = stopwatch.ElapsedMilliseconds / 30;
            return time;
        }
        private long InstrumentStatistics()
        {
            
            var typeStatisticsCollection = db.GetCollection<InstrumentStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);
            var _repository = new InstrumentStatisticsRepository(typeStatisticsCollection, profilesCollectionM);
            
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 30; i++)
            {
                var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult() as InstrumentStatisticsEntity;
            }
            stopwatch.Stop();
            long time = stopwatch.ElapsedMilliseconds / 30;
            return time;
        }
        private long SexStatistics()
        {
            
            var typeStatisticsCollection = db.GetCollection<SexStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);
            var _repository = new SexStatisticsRepository(typeStatisticsCollection, profilesCollectionM);
            
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 30; i++)
            {
                var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult() as SexStatisticsEntity;
            }
            stopwatch.Stop();
            long time = stopwatch.ElapsedMilliseconds / 30;
            return time;
        }
        private long TopStatistics()
        {

            var typeStatisticsCollection = db.GetCollection<TopStylesStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);
            var _repository = new TopStylesStatisticsRepository(typeStatisticsCollection, profilesCollectionM);

            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 30; i++)
            {
                var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult() as TopStylesStatisticsEntity;
            }
            stopwatch.Stop();
            long time = stopwatch.ElapsedMilliseconds / 30;
            return time;
        }
    }
}
