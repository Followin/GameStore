using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.ExpressionPipeline;
using GameStore.BLL.Utils;
using GameStore.Domain.Entities;

namespace GameStore.BLL.GameExpressionPipeline.Predicates
{
    public class PlatformIdsGamePredicate  : BaseGameExpression
    {
        private IEnumerable<Int32> _genreIds;

        public PlatformIdsGamePredicate(IEnumerable<int> genreIds)
        {
            _genreIds = genreIds;
        }

        protected override Expression<Func<Game, bool>> _Execute(Expression<Func<Game, bool>> item)
        {
            return item.AndAlso(game => _genreIds.Intersect(game.Genres.Select(x => x.Id)).Any());
        }
    }
}
