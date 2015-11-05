using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GameStore.Domain.Abstract
{
    public interface IRepository<TEntity, in TKey> where TEntity : Entity<TKey>
    {
        /// <summary>
        /// Get's entity item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entry if found. Null if item wasn't found.</returns>
        TEntity Get(TKey id);

        /// <summary>
        /// Get all items
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> Get();

        /// <summary>
        /// Get all items matching predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, Boolean>> predicate);
            /// <summary>
        /// Gets first item matching predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity GetSingle(Expression<Func<TEntity, Boolean>> predicate);

        Int32 GetCount(Expression<Func<TEntity, Boolean>> predicate = null);

        void Add(TEntity item);

        void Delete(TKey id);

        void Update(TEntity item);
    }
}
