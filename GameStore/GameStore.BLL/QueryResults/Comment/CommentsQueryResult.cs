using System;
using System.Collections;
using System.Collections.Generic;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults.Comment
{
    public class CommentsQueryResult : IEnumerable<CommentDTO>, IQueryResult
    {
        private IEnumerable<CommentDTO> _comments;

        public int GameId { get; set; }

        public bool GameIsDeleted { get; set; }

        public CommentsQueryResult(IEnumerable<CommentDTO> comments)
        {
            _comments = comments;
        }

        public IEnumerator<CommentDTO> GetEnumerator()
        {
            return _comments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
