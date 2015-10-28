using System;
using System.Linq.Expressions;
using GameStore.BLL.ExpressionPipeline;
using GameStore.BLL.Utils;
using GameStore.Domain.Entities;

namespace GameStore.BLL.GameExpressionPipeline.Predicates
{
    public class MinPublishDateGamePredicate : BaseGameExpression
    {
        private DateTime _minDate;

        public MinPublishDateGamePredicate(DateTime minDate)
        {
            _minDate = minDate;
        }


        protected override Expression<Func<Game, bool>> _Execute(Expression<Func<Game, bool>> item)
        {
            return item.AndAlso(game => game.PublicationDate > _minDate);
        }
    }
}
