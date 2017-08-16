using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Funkmap.Common.Data.Mongo.Abstract
{
    public interface IMongoRepository<T> where T : class 
    {
        Task<ICollection<T>> GetAllAsync();
        Task<T> GetAsync(string id);
        Task CreateAsync(T item);
        Task<DeleteResult> DeleteAsync(string id);
        Task UpdateAsync(T entity);
    }
}
