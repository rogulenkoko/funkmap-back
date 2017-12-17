using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories.Abstract;
using Funkmap.Statistics.Mappers;
using Funkmap.Statistics.Models;
using Funkmap.Statistics.Models.Requests;

namespace Funkmap.Statistics.Services
{
    public interface IStatisticsBuilder
    {
        Task<ProfileStatistics> BuildProfileStatisticsAsync();
        Task<MusicianStatistics> BuildMusicianStatisticsAsync();

        Task<ProfileStatistics> BuildProfileStatisticsAsync(DateRequest request);
        Task<MusicianStatistics> BuildMusicianStatisticsAsync(DateRequest request);

        Task<ICollection<IStatistics>> BuildSpecificStatistic(StatisticsType type);

    }

    public class StatisticsBuilder : IStatisticsBuilder
    {
        private readonly IEnumerable<IProfileStatisticsRepository> _profileStatisticsRepositories;
        private readonly IEnumerable<IMusicianStatisticsRepository> _musicianStatisticsRepositories;

        private readonly IBaseStatisticsRepository _baseStatisticsRepository;

        public StatisticsBuilder(IEnumerable<IProfileStatisticsRepository> profileStatisticsRepositories, IEnumerable<IMusicianStatisticsRepository> musicianStatisticsRepositories,
            IBaseStatisticsRepository baseStatisticsRepository)
        {
            _profileStatisticsRepositories = profileStatisticsRepositories;
            _musicianStatisticsRepositories = musicianStatisticsRepositories;
            _baseStatisticsRepository = baseStatisticsRepository;
        }

        public async Task<ProfileStatistics> BuildProfileStatisticsAsync()
        {
            var statisticsDictionary = new Dictionary<StatisticsType, BaseStatisticsEntity>();

            var profileStatisticsTypes = _profileStatisticsRepositories.Select(x => x.StatisticsType).ToArray();
            var profileStatistics = await _baseStatisticsRepository.GetAllStatisticsAsync(profileStatisticsTypes);

            var now = DateTime.UtcNow;

            foreach (var statistic in profileStatistics)
            {
                var repository = _profileStatisticsRepositories.SingleOrDefault(x => x.StatisticsType == statistic.StatisticsType);
                if (repository == null) continue;

                var lastStatistics = await repository.BuildStatisticsAsync(statistic.LastUpdate, now);

                var buildedStatistics = statistic.Merge(lastStatistics);

                statisticsDictionary.Add(buildedStatistics.StatisticsType, buildedStatistics);
            }

            return new ProfileStatistics()
            {
                CityStatistics = statisticsDictionary.ContainsKey(StatisticsType.City) ? (statisticsDictionary[StatisticsType.City] as CityStatisticsEntity).ToModel() : null,
                TopProfileStatistics = statisticsDictionary.ContainsKey(StatisticsType.TopEntity) ? (statisticsDictionary[StatisticsType.TopEntity] as TopProfileStatisticsEntity).ToModel() : null,
                ProfileTypeStatistics = statisticsDictionary.ContainsKey(StatisticsType.EntityType) ? (statisticsDictionary[StatisticsType.EntityType] as EntityTypeStatisticsEntity).ToModel() : null,
             };
        }

        public async Task<ProfileStatistics> BuildProfileStatisticsAsync(DateRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var statisticsDictionary = new Dictionary<StatisticsType, BaseStatisticsEntity>();
            
            foreach (var repository in _profileStatisticsRepositories)
            {
                var buildedStatistics = await repository.BuildStatisticsAsync(request.Begin, request.End);

                statisticsDictionary.Add(buildedStatistics.StatisticsType, buildedStatistics);
            }

            return new ProfileStatistics()
            {
                CityStatistics = statisticsDictionary.ContainsKey(StatisticsType.City) ? (statisticsDictionary[StatisticsType.City] as CityStatisticsEntity).ToModel() : null,
                TopProfileStatistics = statisticsDictionary.ContainsKey(StatisticsType.TopEntity) ? (statisticsDictionary[StatisticsType.TopEntity] as TopProfileStatisticsEntity).ToModel() : null,
                ProfileTypeStatistics = statisticsDictionary.ContainsKey(StatisticsType.EntityType) ? (statisticsDictionary[StatisticsType.EntityType] as EntityTypeStatisticsEntity).ToModel() : null,
            };
        }

        public async Task<MusicianStatistics> BuildMusicianStatisticsAsync()
        {
            var statisticsDictionary = new Dictionary<StatisticsType, BaseStatisticsEntity>();
            var musicianStatisticsTypes = _musicianStatisticsRepositories.Select(x => x.StatisticsType).ToArray();
            var musicianStatistics = await _baseStatisticsRepository.GetAllStatisticsAsync(musicianStatisticsTypes);
            
            var now = DateTime.UtcNow;

            foreach (var statistic in musicianStatistics)
            {
                var repository = _musicianStatisticsRepositories.SingleOrDefault(x => x.StatisticsType == statistic.StatisticsType);
                if (repository == null) continue;

                var lastStatistics = await repository.BuildStatisticsAsync(statistic.LastUpdate, now);

                var buildedStatistics = statistic.Merge(lastStatistics);

                statisticsDictionary.Add(buildedStatistics.StatisticsType, buildedStatistics);
            }

            return new MusicianStatistics()
            {
                AgeStatistics = statisticsDictionary.ContainsKey(StatisticsType.Age) ? (statisticsDictionary[StatisticsType.Age] as AgeStatisticsEntity).ToModel() : null,
                StyleStatistics = statisticsDictionary.ContainsKey(StatisticsType.TopStyles) ? (statisticsDictionary[StatisticsType.TopStyles] as TopStylesStatisticsEntity).ToModel() : null,
                SexStatistics = statisticsDictionary.ContainsKey(StatisticsType.SexType) ? (statisticsDictionary[StatisticsType.SexType] as SexStatisticsEntity).ToModel() : null,
                InstrumentStatistics = statisticsDictionary.ContainsKey(StatisticsType.InstrumentType) ? (statisticsDictionary[StatisticsType.InstrumentType] as InstrumentStatisticsEntity).ToModel() : null,
                BandStatistics = statisticsDictionary.ContainsKey(StatisticsType.InBand) ? (statisticsDictionary[StatisticsType.InBand] as InBandStatisticsEntity).ToModel() : null,
            };
        }

        public async Task<MusicianStatistics> BuildMusicianStatisticsAsync(DateRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var statisticsDictionary = new Dictionary<StatisticsType, BaseStatisticsEntity>();

            foreach (var repository in _musicianStatisticsRepositories)
            {
                var buildedStatistics = await repository.BuildStatisticsAsync(request.Begin, request.End);

                statisticsDictionary.Add(buildedStatistics.StatisticsType, buildedStatistics);
            }

            return new MusicianStatistics()
            {
                AgeStatistics = statisticsDictionary.ContainsKey(StatisticsType.Age) ? (statisticsDictionary[StatisticsType.Age] as AgeStatisticsEntity).ToModel() : null,
                StyleStatistics = statisticsDictionary.ContainsKey(StatisticsType.TopStyles) ? (statisticsDictionary[StatisticsType.TopStyles] as TopStylesStatisticsEntity).ToModel() : null,
                SexStatistics = statisticsDictionary.ContainsKey(StatisticsType.SexType) ? (statisticsDictionary[StatisticsType.SexType] as SexStatisticsEntity).ToModel() : null,
                InstrumentStatistics = statisticsDictionary.ContainsKey(StatisticsType.InstrumentType) ? (statisticsDictionary[StatisticsType.InstrumentType] as InstrumentStatisticsEntity).ToModel() : null,
                BandStatistics = statisticsDictionary.ContainsKey(StatisticsType.InBand) ? (statisticsDictionary[StatisticsType.InBand] as InBandStatisticsEntity).ToModel() : null,
            };
        }

        public async Task<ICollection<IStatistics>> BuildSpecificStatistic(StatisticsType type)
        {
            IStatisticsRepository repository = _profileStatisticsRepositories.SingleOrDefault(x => x.StatisticsType == type);
            if (repository == null)
            {
                repository = _musicianStatisticsRepositories.SingleOrDefault(x => x.StatisticsType == type);
            }

            if(repository == null) throw new InvalidOperationException("Некорректный тип статистик");

            var statistics = await repository.BuildFullStatisticsAsync();
            var statisticsModels = statistics.ToStatisticsModel(type);
            return statisticsModels;
        }
    }
}
