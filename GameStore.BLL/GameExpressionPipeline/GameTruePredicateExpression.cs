using System;
using System.Linq.Expressions;
using GameStore.Domain.Entities;

namespace GameStore.BLL.ExpressionPipeline
{
    public class GameTruePredicateExpression : BaseGameExpression
    {
        protected override Expression<Func<Game, bool>> _Execute(Expression<Func<Game, bool>> item)
        {
            return game => true;
        }
    }
}
