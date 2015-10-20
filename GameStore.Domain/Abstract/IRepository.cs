using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameStore.Domain.Abstract.Entities;

namespace GameStore.Domain.Abstract
{
    public interface IRepository<TEntity, in TKey> where TEntity : Entity<TKey>
    {
        TEntity Get(TKey id);
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Expression<Func<TEntity, Boolean>> predicate);
        TEntity GetSingle(Expression<Func<TEntity, Boolean>> predicate);
        void Add(TEntity item);
        void Delete(TKey id);
        void Update(TEntity item);
        
    }
}
