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
    public class PublisherIdsGamePredicate : BaseGameExpression
    {
        private IEnumerable<Int32> _publisherIds;

        public PublisherIdsGamePredicate(IEnumerable<int> publisherIds)
        {
            _publisherIds = publisherIds;
        }

        protected override Expression<Func<Game, bool>> _Execute(Expression<Func<Game, bool>> item)
        {
            return item.AndAlso(game => _publisherIds.Contains(game.PublisherId));
        }
    }
}
