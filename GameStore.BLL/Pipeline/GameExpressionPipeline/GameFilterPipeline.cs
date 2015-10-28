using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.Static;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.BLL.Pipeline.GameExpressionPipeline
{
    public class GameFilterPipeline
    {
        private IGameStoreUnitOfWork _db;

        public GameFilterPipeline(IGameStoreUnitOfWork db)
        {
            _db = db;
        }

        public IEnumerable<Game> Execute(GetGamesQuery query)
        {
            ITargetPipelineBlock<Expression<Func<Game, Boolean>>> _root = null;
            ISourcePipelineBlock<Expression<Func<Game, Boolean>>> _previous = null;
            IEnumerable<Game> result = null;

            foreach (var expressionPart in GetExpressionPart(query))
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

            var filterTransformBlock = new TransformPipelineBlock<Expression<Func<Game, Boolean>>, IEnumerable<Game>>(
                expr => query.OrderBy != null
                    ? _db.Games.Get(expr, GameOrderTypesList.GetOrderExpression(query.OrderBy))
                    : _db.Games.Get(expr));
            var action =
                new ActionPipelineBlock<IEnumerable<Game>>(games => result = games);
            
            _previous.Register(filterTransformBlock);
            filterTransformBlock.Register(action);

            _root.Post(game => true);
            return result;

        }




        public static IEnumerable<ITransformPipelineBlock<Expression<Func<Game, Boolean>>, Expression<Func<Game, Boolean>>>>
            GetExpressionPart(GetGamesQuery query)
        {
            yield return new TransformPipelineBlock<Expression<Func<Game, bool>>, Expression<Func<Game, bool>>>(
                expr => game => true);

            if (query.GenreIds != null && query.GenreIds.Any())
            {
                yield return new TransformPipelineBlock<Expression<Func<Game, bool>>, Expression<Func<Game, bool>>>(
                    expr => expr.AndAlso(game => query.GenreIds.Intersect(game.Genres.Select(x => x.Id)).Any()));
            }

            if (query.PlatformTypeIds != null && query.PlatformTypeIds.Any())
            {
                yield return new TransformPipelineBlock<Expression<Func<Game, bool>>, Expression<Func<Game, bool>>>(
                    expr => expr.AndAlso(game => query.PlatformTypeIds.Intersect(game.PlatformTypes.Select(x => x.Id)).Any()));
            }

            if (query.PublisherIds != null && query.PublisherIds.Any())
            {
                yield return new TransformPipelineBlock<Expression<Func<Game, bool>>, Expression<Func<Game, bool>>>(
                    expr => expr.AndAlso(game => query.PlatformTypeIds.Contains(game.PublisherId)));
            }

            if (query.MinDate.HasValue)
            {
                yield return new TransformPipelineBlock<Expression<Func<Game, bool>>, Expression<Func<Game, bool>>>(
                    expr => expr.AndAlso(game => game.PublicationDate > query.MinDate.Value));
            }

            if (query.MinPrice > 0)
            {
                yield return new TransformPipelineBlock<Expression<Func<Game, bool>>, Expression<Func<Game, bool>>>(
                    expr => expr.AndAlso(game => game.Price > query.MinPrice));
            }

            if (query.MaxPrice > 0)
            {
                yield return new TransformPipelineBlock<Expression<Func<Game, bool>>, Expression<Func<Game, bool>>>(
                    expr => expr.AndAlso(game => game.Price < query.MaxPrice));
            }

            if (!String.IsNullOrWhiteSpace(query.Name))
            {
                yield return new TransformPipelineBlock<Expression<Func<Game, bool>>, Expression<Func<Game, bool>>>(
                    expr => expr.AndAlso(game => 
                        game.Name.IndexOf(query.Name, StringComparison.InvariantCultureIgnoreCase) > -1));
            }


        }

    }
}
