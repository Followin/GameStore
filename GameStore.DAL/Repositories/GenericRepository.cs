using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract;

namespace GameStore.DAL.Repositories
{
    public class GenericRepository<T> : IRepository<T, Int32> 
        where T : Entity<Int32>
    {
        private IDbSet<T> set;
        private IContext db;

        public GenericRepository(IContext context)
        {
            db = context;
            set = db.Set<T>();
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return set.FirstOrDefault(predicate);
        }

        public void Add(T item)
        {
            set.Add(item);
        }

        public void Delete(Int32 id)
        {
            var item = set.Find(id);
            if (item != null)
            {
                set.Remove(item);
            }
        }

        public IEnumerable<T> Get()
        {
            return set;
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return set.Where(predicate);
        }

        public T Get(Int32 id)
        {
            return set.Find(id);
        }

        public void Update(T item)
        {
            db.SetModified(item);
        }
    }
}
