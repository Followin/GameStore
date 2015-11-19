using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.Comment;
using GameStore.BLL.QueryResults.Comment;
using GameStore.BLL.Utils;
using GameStore.DAL.Abstract;
using NLog;
using EntryState = GameStore.Domain.Entities.EntryState;

namespace GameStore.BLL.QueryHandlers.Comment
{
    public class GetCommentsForGameQueryHandler : 
        IQueryHandler<GetCommentsForGameQuery, CommentsQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetCommentsForGameQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CommentsQueryResult Retrieve(GetCommentsForGameQuery query)
        {
            query.Key.Argument(NameGetter.GetName(() => query.Key))
                 .NotWhiteSpace();

            var game = query.Key != null 
                ? _db.Games.GetFirst(g => g.Key == query.Key) 
                : _db.Games.Get(query.Id);

            if (game == null)
            {
                throw new EntityNotFoundException(
                    "Game with such key wasn't found",
                    NameGetter.GetName(() => query.Key));
            }

            var comments = _db.Comments.Get(c => c.GameId == game.Id
                && c.ParentComment == null
                && c.EntryState == EntryState.Active).ToList();

            RemoveDeletedComments(comments);

            var commentsList = Mapper.Map<IEnumerable<Domain.Entities.Comment>, IEnumerable<CommentDTO>>(comments);

            return new CommentsQueryResult(commentsList) {GameId = game.Id, GameIsDeleted = game.EntryState == EntryState.Deleted};
        }

        private void RemoveDeletedComments(IEnumerable<Domain.Entities.Comment> commentList)
        {
            foreach (var comment in commentList)
            {
                if (comment.ChildComments == null) continue;

                RemoveDeletedComments(comment.ChildComments);

                var toDelete =
                    comment.ChildComments.Where(childComment => childComment.EntryState == EntryState.Deleted).ToList();
                while (toDelete.Any())
                {
                    comment.ChildComments.Remove(toDelete.First());
                    toDelete.Remove(toDelete.First());
                }

                
            }
        }
    }
}
