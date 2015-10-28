using System;
using System.Linq.Expressions;
using GameStore.BLL.ExpressionPipeline;
using GameStore.BLL.Utils;
using GameStore.Domain.Entities;

namespace GameStore.BLL.GameExpressionPipeline.Predicates
{
    public class MinPriceGamePredicate : BaseGameExpression
    {
        private Decimal _minPrice;

        public MinPriceGamePredicate(decimal minPrice)
        {
            _minPrice = minPrice;
        }

        protected override Expression<Func<Game, bool>> _Execute(Expression<Func<Game, bool>> item)
        {
            return item.AndAlso(game => game.Price > _minPrice);
        }
    }
}
