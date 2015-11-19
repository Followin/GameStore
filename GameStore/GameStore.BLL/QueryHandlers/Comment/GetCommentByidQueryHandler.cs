using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Comment;
using GameStore.BLL.QueryResults.Comment;
using GameStore.DAL.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.QueryHandlers.Comment
{
    public class GetCommentByIdQueryHandler :
        IQueryHandler<GetCommentByIdQuery, CommentQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetCommentByIdQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CommentQueryResult Retrieve(GetCommentByIdQuery query)
        {
            var comment = _db.Comments.Get(query.Id);

            if (comment == null)
                return null;

            RemoveDeletedComments(new[] {comment});

            return Mapper.Map<Domain.Entities.Comment, CommentQueryResult>(comment);
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
