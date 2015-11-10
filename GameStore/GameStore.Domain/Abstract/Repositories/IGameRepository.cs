using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GameStore.Domain.Entities;
using GameStore.Static;

namespace GameStore.Domain.Abstract.Repositories
{
    public interface IGameRepository : IRepository<Game, Int32>
    {
        /// <summary>
        /// Get games matching options
        /// </summary>
        /// <param name="predicate">Predicate to match</param>
        /// <param name="comparer">Name of the field to order by</param>
        /// <param name="skip">Items number to skip</param>
        /// <param name="number">Items numer to take after skip</param>
        /// <returns>games list</returns>
        IEnumerable<Game> Get(
            Expression<Func<Game, Boolean>> predicate,
            GamesOrderType orderBy,
            Int32? skip = null,
            Int32? number = null);

        /// <summary>
        /// Get game by key field
        /// </summary>
        /// <param name="key">Game key</param>
        /// <returns>Full game obj</returns>
        Game GetByKey(String key);
    }
}
