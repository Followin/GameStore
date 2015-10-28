using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.BLL.GameExpressionPipeline;
using GameStore.BLL.GameExpressionPipeline.Predicates;
using GameStore.BLL.Queries.Game;
using GameStore.Domain.Entities;

namespace GameStore.BLL.ExpressionPipeline
{
    public static class GameFilterPipeline
    {
        public static Expression<Func<Game, Boolean>> Execute(GetGamesQuery query)
        {
            IExpression<Expression<Func<Game, Boolean>>> _root = null;
            IExpression<Expression<Func<Game, Boolean>>> _previous = null;

            foreach (var expressionPart in _GetExpressionPart(query))
            {
                if (_root == null)
                {
                    _root = expressionPart;
                }
                else
                {
                    _previous.Register(expressionPart);
                }
                _previous = expressionPart;
            }

            return _root == null
                ? game => true
                : _root.Execute(game => true);
        }

        public static IEnumerable<IExpression<Expression<Func<Game, Boolean>>>> _GetExpressionPart(GetGamesQuery query)
        {
            yield return new GameTruePredicateExpression();

            if (query.GenreIds != null && query.GenreIds.Any())
            {
                yield return new GenreIdsGamePredicate(query.GenreIds);
            }

            if (query.PlatformTypeIds != null && query.PlatformTypeIds.Any())
            {
                yield return new PlatformIdsGamePredicate(query.PlatformTypeIds);
            }

            if (query.PublisherIds != null && query.PublisherIds.Any())
            {
                yield return new PublisherIdsGamePredicate(query.PublisherIds);
            }

            if (query.MinDate.HasValue)
            {
                yield return new MinPublishDateGamePredicate(query.MinDate.Value);
            }

            if (query.MinPrice > 0)
            {
                yield return new MinPriceGamePredicate(query.MinPrice);
            }

            if (query.MaxPrice > 0)
            {
                yield return new MaxPriceGamePredicate(query.MaxPrice);
            }

            if (!String.IsNullOrWhiteSpace(query.Name))
            {
                yield return new NameGamePredicate(query.Name);
            }


        }
    }
}
