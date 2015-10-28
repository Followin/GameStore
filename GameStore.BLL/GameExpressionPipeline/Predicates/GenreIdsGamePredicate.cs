using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.BLL.ExpressionPipeline;
using GameStore.BLL.Utils;
using GameStore.Domain.Entities;

namespace GameStore.BLL.GameExpressionPipeline.Predicates
{
    public class GenreIdsGamePredicate : BaseGameExpression
    {
        private IEnumerable<Int32> genreIds;

        public GenreIdsGamePredicate(IEnumerable<int> genreIds)
        {
            this.genreIds = genreIds;
        }

        protected override Expression<Func<Game, bool>> _Execute(Expression<Func<Game, bool>> item)
        {
            return item.AndAlso(game => genreIds.Intersect(game.Genres.Select(x => x.Id)).Any());
        }
    }
}
