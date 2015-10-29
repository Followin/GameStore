using System;
using System.Collections.Generic;
using System.Linq;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Comment;
using GameStore.BLL.QueryResults;
using GameStore.BLL.QueryResults.Comment;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;
using EntryState = GameStore.Domain.Abstract.EntryState;

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
            _db = db;
            _logger = logger;
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

            var comments = _db.Comments.Get(c => c.GameId == game.Id 
                && c.ParentComment == null 
                && c.EntryState == EntryState.Active);

            Action<IEnumerable<Comment>> removeDeletedComments = null;
            removeDeletedComments = commentList =>
            {
                foreach (var comment in commentList)
                {
                    if (comment.ChildComments == null) continue;
                    var toDelete =
                        comment.ChildComments.Where(childComment => childComment.EntryState == EntryState.Deleted);
                    while (toDelete.Any())
                    {
                        comment.ChildComments.Remove(toDelete.First());
                    }
                    removeDeletedComments(comment.ChildComments);
                }
            };
            removeDeletedComments(comments);

            var commentsList = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(comments);

            return new CommentsQueryResult(commentsList);
        }
    }
}
