using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Objects;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
{//todo нужно доделать
    public class TopStylesStatisticsRepository : MongoRepository<TopStylesStatisticsEntity>, IStatisticsRepository
=======
{
    public class TopStylesStatisticsRepository : MongoRepository<TopStylesStatisticsEntity>, IMusicianStatisticsRepository
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
=======
{
    public class TopStylesStatisticsRepository : MongoRepository<TopStylesStatisticsEntity>, IMusicianStatisticsRepository
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
=======
{
    public class TopStylesStatisticsRepository : MongoRepository<TopStylesStatisticsEntity>, IMusicianStatisticsRepository
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
=======
{
    public class TopStylesStatisticsRepository : MongoRepository<TopStylesStatisticsEntity>, IMusicianStatisticsRepository
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
    {
        private readonly IMongoCollection<MusicianEntity> _profileCollection;
        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
            //db.bases.aggregate(
            //{$unwind: "$stls"},
            //{$project: { stls: 1} },
            //{$group: { _id: "$stls", count: {$sum: 1} } }
            //)

            var filter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician);

            var projection = Builders<MusicianEntity>.Projection.Include(x => x.Styles);

            var statistics = await _profileCollection.Aggregate()
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
                
                .Group(x => x, entities => new CountStatisticsEntity<Styles>()
                {
                    //Key = entities.Key.Styles,
                    Count = entities.GetEnumerator().Current.FavoriteFor.Count
                }).ToListAsync();
            var statistic = new TopEntityStatisticsEntity()
=======
                .Match(filter)
                .Project<MusicianEntity>(projection)
                .Unwind<MusicianEntity, UnwindStyles>(x => x.Styles)
                .Group(x => x.Style, unwinded => new CountStatisticsEntity<Styles>()
                {
=======
                .Match(filter)
                .Project<MusicianEntity>(projection)
                .Unwind<MusicianEntity, UnwindStyles>(x => x.Styles)
                .Group(x => x.Style, unwinded => new CountStatisticsEntity<Styles>()
                {
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
=======
                .Match(filter)
                .Project<MusicianEntity>(projection)
                .Unwind<MusicianEntity, UnwindStyles>(x => x.Styles)
                .Group(x => x.Style, unwinded => new CountStatisticsEntity<Styles>()
                {
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
=======
                .Match(filter)
                .Project<MusicianEntity>(projection)
                .Unwind<MusicianEntity, UnwindStyles>(x => x.Styles)
                .Group(x => x.Style, unwinded => new CountStatisticsEntity<Styles>()
                {
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
                    Key =  unwinded.Key,
                    Count = unwinded.Count()
                })
                .ToListAsync();

            var statistic = new TopStylesStatisticsEntity()
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
=======
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
=======
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
=======
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
            {
                CountStatistics = statistics
            };
            return statistic;
        }

        public async Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            var filter = Builders<MusicianEntity>.Filter.Gte(x => x.CreationDate, begin) &
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
                         Builders<MusicianEntity>.Filter.Lte(x => x.CreationDate, end);

            var statistics = await _profileCollection.Aggregate()
                .Match(filter)
                .Group(x => x, entities => new CountStatisticsEntity<Styles>()
                {
                    //Key = entities.Key.EntityType,
                    Count = entities.Key.FavoriteFor.Count
                }).ToListAsync();
            var statistic = new TopEntityStatisticsEntity()
=======
                         Builders<MusicianEntity>.Filter.Lte(x => x.CreationDate, end)
                         & Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician);

            var projection = Builders<MusicianEntity>.Projection.Include(x => x.Styles);

            var statistics = await _profileCollection.Aggregate()
                .Match(filter)
                .Project<MusicianEntity>(projection)
                .Unwind<MusicianEntity, UnwindStyles>(x => x.Styles)
                .Group(x => x.Style, unwinded => new CountStatisticsEntity<Styles>()
                {
=======
                         Builders<MusicianEntity>.Filter.Lte(x => x.CreationDate, end)
                         & Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician);

            var projection = Builders<MusicianEntity>.Projection.Include(x => x.Styles);

            var statistics = await _profileCollection.Aggregate()
                .Match(filter)
                .Project<MusicianEntity>(projection)
                .Unwind<MusicianEntity, UnwindStyles>(x => x.Styles)
                .Group(x => x.Style, unwinded => new CountStatisticsEntity<Styles>()
                {
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
=======
                         Builders<MusicianEntity>.Filter.Lte(x => x.CreationDate, end)
                         & Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician);

            var projection = Builders<MusicianEntity>.Projection.Include(x => x.Styles);

            var statistics = await _profileCollection.Aggregate()
                .Match(filter)
                .Project<MusicianEntity>(projection)
                .Unwind<MusicianEntity, UnwindStyles>(x => x.Styles)
                .Group(x => x.Style, unwinded => new CountStatisticsEntity<Styles>()
                {
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
=======
                         Builders<MusicianEntity>.Filter.Lte(x => x.CreationDate, end)
                         & Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician);

            var projection = Builders<MusicianEntity>.Projection.Include(x => x.Styles);

            var statistics = await _profileCollection.Aggregate()
                .Match(filter)
                .Project<MusicianEntity>(projection)
                .Unwind<MusicianEntity, UnwindStyles>(x => x.Styles)
                .Group(x => x.Style, unwinded => new CountStatisticsEntity<Styles>()
                {
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
                    Key = unwinded.Key,
                    Count = unwinded.Count()
                })
                .ToListAsync();

            var statistic = new TopStylesStatisticsEntity()
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
=======
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
=======
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
=======
>>>>>>> 81c1e22dafdddcbd97ab7a4b6564d8ab68c4759d
            {
                CountStatistics = statistics
            };
            return statistic;
        }

        public StatisticsType StatisticsType => StatisticsType.TopEntity;

        public TopStylesStatisticsRepository(IMongoCollection<TopStylesStatisticsEntity> collection,
            IMongoCollection<MusicianEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
        }

        public override async Task UpdateAsync(TopStylesStatisticsEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            entity.LastUpdate = DateTime.UtcNow;
            var result = await _collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);
            if (result == null) await CreateAsync(entity);
        }
    }
}
