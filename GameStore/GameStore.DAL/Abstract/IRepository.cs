using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Abstract
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
            Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets first item matching predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity GetFirst(Expression<Func<TEntity, Boolean>> predicate);

        /// <summary>
        /// Get items count
        /// </summary>
        /// <param name="predicate">predicate for items to match</param>
        /// <returns>Items count</returns>
        Int32 GetCount(Expression<Func<TEntity, Boolean>> predicate = null);

        void Add(TEntity item);

        void Delete(TKey id);

        void Update(TEntity item);
    }
}
