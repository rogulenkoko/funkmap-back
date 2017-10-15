using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Common.Data.Mongo
{
    public abstract class MongoRepository<T> : IMongoRepository<T> where T : class 
    {
        protected readonly IMongoCollection<T> _collection;

        protected MongoRepository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public virtual async Task<T> GetAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", new ObjectId(id));
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task CreateAsync(T item)
        {
            await _collection.InsertOneAsync(item);
        }

        public virtual async Task<T> DeleteAsync(string id)
        {
            return await _collection.FindOneAndDeleteAsync(Builders<T>.Filter.Eq("Id", new ObjectId(id)));
        }

        public abstract Task UpdateAsync(T entity);
    }

    public abstract class MongoLoginRepository<T> : MongoRepository<T> where T: class 
    {
        protected MongoLoginRepository(IMongoCollection<T> collection) : base(collection)
        {
        }

        public override async Task<T> GetAsync(string login)
        {
            var filter = Builders<T>.Filter.Eq("log", login);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public override async Task<T> DeleteAsync(string id)
        {
            return await _collection.FindOneAndDeleteAsync(Builders<T>.Filter.Eq("log", id));
        }
    }
}
