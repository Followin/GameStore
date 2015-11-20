using System;
using System.Linq.Expressions;
using GameStore.Domain.Entities;
using GameStore.Static;

namespace GameStore.BLL.QueryHandlers.Games
{
    class GamesQueryBuilder
    {
        public Expression<Func<Game, bool>> Predicate { get; set; }

        public GamesOrderType OrderBy { get; set; }

        public int? Number { get; set; }

        public int? Skip { get; set; }
    }
}