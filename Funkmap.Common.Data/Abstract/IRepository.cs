using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funkmap.Common.Data.Abstract
{
    public interface IRepository<T> where T: class 
    {
        Task<T> GetAsync(long id);
        Task<IEnumerable<T>> GetAllAsync();
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        Task SaveAsync();
    }
}
