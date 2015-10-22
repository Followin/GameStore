using System.Collections;
using System.Collections.Generic;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults
{
    public class CommentsQueryResult : IEnumerable<CommentDTO>, IQueryResult
    {
        private IEnumerable<CommentDTO> _comments;

        public CommentsQueryResult(IEnumerable<CommentDTO> comments)
        {
            this._comments = comments;
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
