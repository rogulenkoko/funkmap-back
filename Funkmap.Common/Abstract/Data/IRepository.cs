using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Common.Abstract.Data
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
