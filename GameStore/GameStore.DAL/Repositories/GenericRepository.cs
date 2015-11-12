using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.EF;
using GameStore.Domain.Abstract;

namespace GameStore.DAL.Repositories
{
    public class GenericRepository<T, TKey> : IRepository<T, TKey>
        where T : Entity<TKey>
    {
        private IDbSet<T> _set;
        protected EFContext Db;

        public GenericRepository(EFContext context)
        {
            Db = context;
            _set = Db.Set<T>();
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _set.ToList().FirstOrDefault(predicate.Compile());
        }

        public int GetCount(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? _set.Count()
                : _set.Count(predicate);
        }

        public void Add(T item)
        {
            _set.Add(item);
        }

        public void Delete(TKey id)
        {
            var item = _set.Find(id);
            if (item != null)
            {
                _set.Remove(item);
            }
        }

        public IEnumerable<T> Get()
        {
            return _set.ToList();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate).ToList();
        }


        public T Get(TKey id)
        {
            return _set.Find(id);
        }

        public void Update(T item)
        {
            Db.SetModified(item);
        }
    }
}
