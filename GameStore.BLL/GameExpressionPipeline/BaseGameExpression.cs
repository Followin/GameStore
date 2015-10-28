using System;
using System.Linq.Expressions;
using GameStore.Domain.Entities;

namespace GameStore.BLL.ExpressionPipeline
{
    public abstract class BaseGameExpression : IExpression<Expression<Func<Game, Boolean>>>
    {
        private IExpression<Expression<Func<Game, Boolean>>> _nextItem;

        protected abstract Expression<Func<Game, Boolean>> _Execute(Expression<Func<Game, Boolean>> item);

        public Expression<Func<Game, Boolean>> Execute(Expression<Func<Game, Boolean>> item)
        {
            var retVal = _Execute(item);

            if (_nextItem != null)
            {
                retVal = _nextItem.Execute(retVal);
            }

            return retVal;
        }

        public void Register(IExpression<Expression<Func<Game, bool>>> nextItem)
        {
            _nextItem = nextItem;
        }
    }
}
