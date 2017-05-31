using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Funkmap.Common.Abstract.Data;

namespace Funkmap.Common
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            if(context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public void Edit(T entity)
        {
            Context.Set<T>().AddOrUpdate(entity);
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public T Get(int id)
        {
            return Context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>().ToList();
        }
    }
}
