using System;
using System.Linq.Expressions;
using GameStore.BLL.ExpressionPipeline;
using GameStore.BLL.Utils;
using GameStore.Domain.Entities;

namespace GameStore.BLL.GameExpressionPipeline.Predicates
{
    public class NameGamePredicate : BaseGameExpression
    {
        private String _namePart;

        public NameGamePredicate(string namePart)
        {
            _namePart = namePart;
        }

        protected override Expression<Func<Game, bool>> _Execute(Expression<Func<Game, bool>> item)
        {
            return item.AndAlso(x => x.Name.IndexOf(_namePart, StringComparison.InvariantCultureIgnoreCase) > -1);
        }
    }
}
