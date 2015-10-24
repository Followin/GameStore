﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract;

namespace GameStore.DAL.Repositories
{
    public class GenericRepository<T, TKey> : IRepository<T, TKey> 
        where T : Entity<TKey>
    {
        private IDbSet<T> _set;
        private IContext _db;

        public GenericRepository(IContext context)
        {
            _db = context;
            _set = _db.Set<T>();
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _set.FirstOrDefault(predicate);
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
            return _set;
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate);
        }

        public T Get(TKey id)
        {
            return _set.Find(id);
        }

        public void Update(T item)
        {
            _db.SetModified(item);
        }
    }
}
