using System;
using System.Collections.Generic;
using GameStore.Domain.Abstract;

namespace GameStore.DAL.Abstract
{
    public interface IOutRepository<T> where T:Entity<Int32>
    {
        T Get(Int32 id);

        IEnumerable<T> Get();

        IEnumerable<T> GetExluding(IEnumerable<Int32> exludingIds);
    }
}
