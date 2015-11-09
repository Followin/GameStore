﻿using System;
using System.Collections.Generic;
using GameStore.Domain.Abstract;

namespace GameStore.DAL.Abstract
{
    public interface IOutRepository<T> where T:Entity<Int32>
    {
        /// <summary>
        /// Get's an item and maps it to entity
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns></returns>
        T Get(Int32 id);

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
        IEnumerable<T> GetExluding(IEnumerable<Int32> exludingIds);
    }
}