using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Objects;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Data.Services.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Data.Repositories
{
    public class BaseRepository : MongoLoginRepository<BaseEntity>, IBaseRepository
    {
        private readonly IMongoCollection<BaseEntity> _collection;
        private readonly IFilterFactory _filterFactory;

        private const int Radius = 100;

        public BaseRepository(IMongoCollection<BaseEntity> collection,
                              IFilterFactory filterFactory) : base(collection)
        {
            _collection = collection;
            _filterFactory = filterFactory;
        }

        public async Task<ICollection<BaseEntity>> GetAllAsyns()
        {
            var projection = Builders<BaseEntity>.Projection.Exclude(x => x.Photo)
                .Exclude(x => x.Description)
                .Exclude(x => x.Name);
            
            return await _collection.Find(x => true).Project<BaseEntity>(projection).ToListAsync();
        }

        public async Task<ICollection<BaseEntity>> GetNearestAsync(LocationParameter parameter)
        {
            //db.bases.find({ loc: { $nearSphere: [50, 30], $minDistance: 0, $maxDistance: 1 } }).limit(20)

            //todo придумать адекватную проекцию для геопозиции
            var projection = Builders<BaseEntity>.Projection.Exclude(x => x.Photo)
                .Exclude(x => x.Description)
                .Exclude(x => x.Name)
                .Exclude(x => x.Address)
                .Exclude(x => x.VideoInfos)
                .Exclude(x => x.YouTubeLink)
                .Exclude(x => x.FacebookLink)
                .Exclude(x => x.SoundCloudLink)
                .Exclude(x => x.VkLink);


            ICollection<BaseEntity> result;
            if (parameter.Longitude == null || parameter.Latitude == null)
            {
                result = await _collection.Find(x => true).Project<BaseEntity>(projection).Limit(parameter.Take).ToListAsync();
            }
            else
            {
                var filter = Builders<BaseEntity>.Filter.NearSphere(x => x.Location, parameter.Longitude.Value, parameter.Latitude.Value, parameter.RadiusDeg);
                result = await _collection.Find(filter).Limit(parameter.Take).ToListAsync();
            }
            return result;

        }

        public async Task<ICollection<BaseEntity>> GetFullNearestAsync(LocationParameter parameter)
        {
            ICollection<BaseEntity> result;
            if (parameter.Longitude == null || parameter.Latitude == null)
            {
                result = await _collection.Find(x => true).Skip(parameter.Skip).Limit(parameter.Take).ToListAsync();
            }
            else
            {
                var filter = Builders<BaseEntity>.Filter.NearSphere(x => x.Location, parameter.Longitude.Value, parameter.Latitude.Value, parameter.RadiusDeg);
                result = await _collection.Find(filter).Skip(parameter.Skip).Limit(parameter.Take).ToListAsync();
            }

            return result;
        }

        public async Task<ICollection<BaseEntity>> GetSpecificNavigationAsync(string[] logins)
        {
            //todo придумать адекватную проекцию для геопозиции
            var projection = Builders<BaseEntity>.Projection.Exclude(x => x.Photo)
                .Exclude(x => x.Description)
                .Exclude(x => x.Name)
                .Exclude(x=>x.Address)
                .Exclude(x=>x.VideoInfos)
                .Exclude(x=>x.YouTubeLink)
                .Exclude(x=>x.FacebookLink)
                .Exclude(x=>x.SoundCloudLink)
                .Exclude(x=>x.VkLink);

            var filter = Builders<BaseEntity>.Filter.In(x => x.Login, logins);
            var result = await _collection.Find(filter).Project<BaseEntity>(projection).ToListAsync();
            return result;
        }

        public async Task<ICollection<BaseEntity>> GetSpecificFullAsync(string[] logins)
        {
            var filter = Builders<BaseEntity>.Filter.In(x => x.Login, logins);
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }

        public async Task<ICollection<string>> GetUserEntitiesLogins(string userLogin)
        {
            //db.bases.find({"user":"rogulenkoko"},{"log":1})
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.UserLogin, userLogin);
            var projection = Builders<BaseEntity>.Projection.Include(x => x.Login);
            var entities = await _collection.Find(filter).Project<BaseEntity>(projection).ToListAsync();
            var result = entities.Select(x => x.Login).ToList();
            return result;
        }

        public async Task<ICollection<UserEntitiesCountInfo>> GetUserEntitiesCountInfo(string userLogin)
        {
            //db.bases.aggregate([
            //{ $match: { user: "rogulenkoko" } },
            //{ $group: { _id: "$t", count: {$sum: 1} } } 
            //])

            var countResult = await _collection.Aggregate()
                .Match(x => x.UserLogin == userLogin)
                .Group(x=> x.EntityType, y=> new UserEntitiesCountInfo()
                {
                    EntityType = y.Key,
                    Count = y.Count(),
                    Logins = y.Select(x=>x.Login).ToList()
                }).ToListAsync();

            return countResult;
        }

        public async Task<ICollection<BaseEntity>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            var filter = CreateFilter(commonFilter, parameter);

            if (commonFilter.Longitude.HasValue && commonFilter.Latitude.HasValue)
            {
                filter = filter & Builders<BaseEntity>.Filter.NearSphere(x => x.Location, commonFilter.Longitude.Value, commonFilter.Latitude.Value, commonFilter.RadiusDeg);
            }

            var result = await _collection.Find(filter).Skip(commonFilter.Skip).Limit(commonFilter.Take).ToListAsync();
            return result;
        }

        public async Task<ICollection<string>> GetAllFilteredLoginsAsync(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            var filter = CreateFilter(commonFilter, parameter);
            var profection = Builders<BaseEntity>.Projection.Include(x => x.Login);

            if (commonFilter.Longitude.HasValue && commonFilter.Latitude.HasValue)
            {
                filter = filter & Builders<BaseEntity>.Filter.NearSphere(x => x.Location, commonFilter.Longitude.Value, commonFilter.Latitude.Value, commonFilter.RadiusDeg);
            }

            var result = await _collection.Find(filter).Project<BaseEntity>(profection).ToListAsync();
            return result.Select(x=>x.Login).ToList();
        }

        public async Task<bool> CheckIfLoginExistAsync(string login)
        {
            var projection = Builders<BaseEntity>.Projection.Include(x => x.Login);
            var entity = await _collection.Find(x => x.Login == login).Project(projection).FirstOrDefaultAsync();
            return entity != null;
        }

        private FilterDefinition<BaseEntity> CreateFilter(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            var filter = Builders<BaseEntity>.Filter.Empty;

            if (parameter != null)
            {
                filter = filter & _filterFactory.CreateFilter(parameter);
            }

            if (commonFilter.EntityType != 0)
            {
                filter = filter & Builders<BaseEntity>.Filter.Eq(x => x.EntityType, commonFilter.EntityType);
            }

            if (!String.IsNullOrEmpty(commonFilter.SearchText))
            {
                filter = filter & Builders<BaseEntity>.Filter.Regex(x => x.Name, $"/{commonFilter.SearchText}/i");
            }

            if (!String.IsNullOrEmpty(commonFilter.UserLogin))
            {
                filter = filter & Builders<BaseEntity>.Filter.Eq(x => x.UserLogin, commonFilter.UserLogin);
            }

            return filter;
        }

        public override async Task UpdateAsync(BaseEntity entity)
        {
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.Login, entity.Login) & Builders<BaseEntity>.Filter.Eq(x=>x.EntityType, entity.EntityType);

            await _collection.ReplaceOneAsync(filter, entity);
        }
    }
}
