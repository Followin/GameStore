using System;
using System.Collections.Generic;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Comment;
using GameStore.BLL.QueryResults;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.QueryHandlers
{
    public class CommentQueryHandler :
    #region interfaces  
        IQueryHandler<GetCommentsByGameKeyQuery, CommentsQueryResult>
    #endregion
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public CommentQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public CommentsQueryResult Retrieve(GetCommentsByGameKeyQuery query)
        {
            query.Key.Argument(NameGetter.GetName(() => query.Key))
                 .NotNull()
                 .NotWhiteSpace();
            var game = _db.Games.GetSingle(g => g.Key == query.Key);
            if (game == null)
            {
                throw new EntityNotFoundException(
                    "Game with such key wasn't found",
                    NameGetter.GetName(() => query.Key));
            }

            var comments = _db.Comments.Get(c => c.GameId == game.Id && c.ParentComment == null);
            var commentsList = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(comments);

            return new CommentsQueryResult(commentsList);
        }
    }
}
