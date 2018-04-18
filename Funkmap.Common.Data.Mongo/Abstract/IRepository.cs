using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funkmap.Common.Data.Mongo.Abstract
{
    public interface IRepository<T> where T : class 
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(string id);
        Task CreateAsync(T item);
        Task<T> DeleteAsync(string id);
        Task UpdateAsync(T entity);
    }
}
