using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract.Repositories
{
    public interface IGameRepository : IRepository<Game, Int32>
    {
        IEnumerable<Game> Get(
            Expression<Func<Game, Boolean>> predicate,
            String comparer,
            Int32? skip = null,
            Int32? number = null);
    }
}
