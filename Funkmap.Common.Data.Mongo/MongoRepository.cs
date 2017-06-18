using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
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

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public virtual async Task<T> GetAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task CreateAsync(T item)
        {
            await _collection.InsertOneAsync(item);
        }

        public async Task<DeleteResult> DeleteAsync(string id)
        {
            return await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));
        }

        public abstract Task<UpdateResult> UpdateAsync(T entity);
    }

    public abstract class MongoLoginRepository<T> : MongoRepository<T> where T: class 
    {
        public MongoLoginRepository(IMongoCollection<T> collection) : base(collection)
        {
        }

        public async override Task<T> GetAsync(string login)
        {
            var filter = Builders<T>.Filter.Eq("log", login);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }
    }
}
