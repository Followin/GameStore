using System;
using System.Collections.Generic;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries;
using GameStore.BLL.QueryResults;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.QueryHandlers
{
    public class CommentQueryHandler :
        IQueryHandler<GetCommentsByGameKeyQuery, CommentsQueryResult>
    {
        private IGameStoreUnitOfWork db;
        private ILogger logger;

        public CommentQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public CommentsQueryResult Retrieve(GetCommentsByGameKeyQuery query)
        {
            query.Key.Argument("Key")
                 .NotNull()
                 .NotWhiteSpace();
            var game = db.Games.GetSingle(g => g.Key == query.Key);
            if (game == null)
            {
                throw new ArgumentException("Game with such key wasn't found", "Key");
            }

            var comments = db.Comments.Get(_ => _.GameId == game.Id && _.ParentComment == null);
            var commentsList = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(comments);

            return new CommentsQueryResult(commentsList);
        }
    }
}
