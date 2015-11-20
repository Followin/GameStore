using System;
using System.Collections.Generic;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Abstract.Repositories
{
    public interface IOutRepository<T>
    {
        /// <summary>
        /// Get's an item and maps it to entity
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns></returns>
        T Get(int id);

        /// <summary>
        /// Get all items and map them
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Get();

        /// <summary>
        /// Get all items exluding some ids
        /// </summary>
        /// <param name="exludingIds">Ids to exclude</param>
        /// <returns></returns>
        IEnumerable<T> GetExluding(IEnumerable<int> exludingIds);

        /// <summary>
        /// Get only items with 'including' ids
        /// </summary>
        /// <param name="includingIds">Ids to include</param>
        /// <returns></returns>
        IEnumerable<T> GetIncluding(IEnumerable<int> includingIds);
    }
}
