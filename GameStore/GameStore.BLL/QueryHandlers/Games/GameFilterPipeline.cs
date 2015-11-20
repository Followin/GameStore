using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults.Game;
using GameStore.BLL.Utils;
using GameStore.DAL.Abstract;
using GameStore.Domain.Entities;
using Pipeline;
using EntryState = GameStore.Domain.Entities.EntryState;

namespace GameStore.BLL.QueryHandlers.Games
{
    public class GameFilterPipeline
    {
        private IGameStoreUnitOfWork _db;

        public GameFilterPipeline(IGameStoreUnitOfWork db)
        {
            _db = db;
        }

        public GamesPartQueryResult Execute(GetGamesQuery query)
        {
            ITargetPipelineBlock<Expression<Func<Game, bool>>> _root = null;
            ISourcePipelineBlock<Expression<Func<Game, bool>>> _previous = null;


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
            
            var builderTransformBlock = new TransformPipelineBlock<Expression<Func<Game, bool>>, GamesQueryBuilder>(
                expr => new GamesQueryBuilder {Predicate = expr});
            _previous.Register(builderTransformBlock);
            ISourcePipelineBlock<GamesQueryBuilder> _builderBlock = null;
            _builderBlock = builderTransformBlock;

            if (query.OrderBy != null)
            {
                var orderTransformBlock = new TransformPipelineBlock<GamesQueryBuilder, GamesQueryBuilder>(
                    builder =>
                    {
                        builder.OrderBy = query.OrderBy;
                        return builder;
                    });
                _builderBlock.Register(orderTransformBlock);
                _builderBlock = orderTransformBlock;
            }

            if (query.Number.HasValue)
            {
                var itemsTransformBlock = new TransformPipelineBlock<GamesQueryBuilder, GamesQueryBuilder>(
                    builder =>
                    {
                        builder.Number = query.Number.Value;
                        return builder;
                    });
                _builderBlock.Register(itemsTransformBlock);
                _builderBlock = itemsTransformBlock;
            }

            if (query.Skip.HasValue)
            {
                var skipTransformBlock = new TransformPipelineBlock<GamesQueryBuilder, GamesQueryBuilder>(
                    builder =>
                    {
                        builder.Skip = query.Skip.Value;
                        return builder;
                    });
                _builderBlock.Register(skipTransformBlock);
                _builderBlock = skipTransformBlock;
            }

            GamesPartQueryResult result = null;

            var action =
                new ActionPipelineBlock<GamesQueryBuilder>(buildedQuery =>
                {
                    var games = _db.Games.Get(buildedQuery.Predicate, buildedQuery.OrderBy,
                        buildedQuery.Skip, buildedQuery.Number);
                    result = new GamesPartQueryResult(
                        Mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(games),
                        _db.Games.GetCount(buildedQuery.Predicate));
                });
            _builderBlock.Register(action);

            _root.Post(game => true);
            return result;

        }


        static IEnumerable<ITransformPipelineBlock<Expression<Func<Game, bool>>, Expression<Func<Game, bool>>>>
            GetExpressionPart(GetGamesQuery query)
        {
            yield return new TransformPipelineBlock<Expression<Func<Game, bool>>, Expression<Func<Game, bool>>>(
                expr => game => game.EntryState == EntryState.Active);

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
                    expr => expr.AndAlso(game => game.PublisherId.HasValue && query.PublisherIds.Contains(game.PublisherId.Value)));
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

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                yield return new TransformPipelineBlock<Expression<Func<Game, bool>>, Expression<Func<Game, bool>>>(
                    expr => expr.AndAlso(game => game.Name.ToLower().Contains(query.Name)));
            }


        }

    }
}
