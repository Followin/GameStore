using System;
using System.Linq.Expressions;
using GameStore.BLL.ExpressionPipeline;
using GameStore.BLL.Utils;
using GameStore.Domain.Entities;

namespace GameStore.BLL.GameExpressionPipeline.Predicates
{
    public class MaxPriceGamePredicate : BaseGameExpression
    {
        private Decimal _maxPrice;

        public MaxPriceGamePredicate(decimal maxPrice)
        {
            _maxPrice = maxPrice;
        }

        protected override Expression<Func<Game, bool>> _Execute(Expression<Func<Game, bool>> item)
        {
            return item.AndAlso(game => game.Price < _maxPrice);
        }
    }
}
