using System;
using System.Collections.Generic;

namespace GameStore.Domain.Abstract
{
    public interface IQueryRepository<out TEntity, in TKey>
    {
        TEntity Get(TKey id);

        IEnumerable<TEntity> Get();

        IEnumerable<TEntity> Get(Func<TEntity, Boolean> predicate);

        TEntity GetSingle(Func<TEntity, Boolean> predicate);
    }
}
