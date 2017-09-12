using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Objects;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Data.Services.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly IMongoCollection<BaseEntity> _collection;
        private readonly IFilterFactory _filterFactory;

        public BaseRepository(IMongoCollection<BaseEntity> collection,
                              IFilterFactory filterFactory)
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

            var center = new[] { parameter.Longitude, parameter.Latitude };
            var centerQueryArray = new BsonArray { new BsonArray(center), parameter.RadiusDeg };


            var projection = Builders<BaseEntity>.Projection.Exclude(x => x.Photo)
                .Exclude(x => x.Description)
                .Exclude(x => x.Name);

            ICollection<BaseEntity> result;
            if (parameter.Longitude == null || parameter.Latitude == null)
            {
                result = await _collection.Find(x => true).Project<BaseEntity>(projection).ToListAsync();
            }
            else
            {
                var filter = new BsonDocument("loc", new BsonDocument("$within", new BsonDocument("$center", centerQueryArray)));
                result = await _collection.Find(filter).Project<BaseEntity>(projection).ToListAsync();
            }
            return result;

        }

        public async Task<ICollection<BaseEntity>> GetFullNearestAsync(FullLocationParameter parameter)
        {
            var center = new[] { parameter.Longitude, parameter.Latitude };
            var centerQueryArray = new BsonArray { new BsonArray(center), parameter.RadiusDeg };

            ICollection<BaseEntity> result;
            if (parameter.Longitude == null || parameter.Latitude == null)
            {
                result = await _collection.Find(x => true).Skip(parameter.Skip).Limit(parameter.Take).ToListAsync();
            }
            else
            {
                var filter = new BsonDocument("loc", new BsonDocument("$within", new BsonDocument("$center", centerQueryArray)));
                result = await _collection.Find(filter).Skip(parameter.Skip).Limit(parameter.Take).ToListAsync();
            }

            return result;
        }

        public async Task<ICollection<BaseEntity>> GetSpecificAsync(string[] logins)
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

        public async Task<ICollection<BaseEntity>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            var filter = CreateFilter(commonFilter, parameter);
            var result = await _collection.Find(filter).Skip(commonFilter.Skip).Limit(commonFilter.Take).ToListAsync();
            return result;
        }

        public async Task<ICollection<string>> GetAllFilteredLoginsAsync(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            var filter = CreateFilter(commonFilter, parameter);
            var result = await _collection.Find(filter).ToListAsync();
            return result.Select(x=>x.Login).ToList();
        }

        public async Task<bool> CheckIfLoginExistAsync(string login)
        {
            var projection = Builders<BaseEntity>.Projection.Include(x => x.Login);
            var entity = await _collection.Find(x => x.Login == login).Project(projection).FirstOrDefaultAsync();
            return entity != null;
        }

        public async Task ChangeAvatarAsync(ChangeAvatarParameter parameter)
        {
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.Login, parameter.Login)
                         & Builders<BaseEntity>.Filter.Eq(x => x.UserLogin, parameter.UserLogin);

            var update = Builders<BaseEntity>.Update.Set(x => x.Photo, parameter.Avatar);

            await _collection.FindOneAndUpdateAsync(filter, update);
        }

        private FilterDefinition<BaseEntity> CreateFilter(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            var filter = _filterFactory.CreateFilter(parameter);

            if (commonFilter.EntityType != 0)
            {
                filter = filter & Builders<BaseEntity>.Filter.Eq(x => x.EntityType, commonFilter.EntityType);
            }

            if (!String.IsNullOrEmpty(commonFilter.SearchText))
            {
                filter = filter & Builders<BaseEntity>.Filter.Regex(x => x.Name, $"/{commonFilter.SearchText}/i");
            }
            return filter;
        }

        public async Task UpdateAsync(BaseEntity entity)
        {
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.Login, entity.Login) & Builders<BaseEntity>.Filter.Eq(x=>x.EntityType, entity.EntityType);

            await _collection.ReplaceOneAsync(filter, entity);
        }
    }
}
