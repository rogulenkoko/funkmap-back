using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Common.Data.Mongo
{
    public abstract class RepositoryBase<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        protected RepositoryBase(IMongoCollection<T> collection)
        {
            _collection = collection;
        }
    }

    public abstract class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class 
    {

        protected Repository(IMongoCollection<T> collection) : base(collection) { }

        public virtual async Task<List<T>> GetAllAsync()
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

    public abstract class LoginRepository<T> : Repository<T> where T: class 
    {
        protected LoginRepository(IMongoCollection<T> collection) : base(collection)
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
