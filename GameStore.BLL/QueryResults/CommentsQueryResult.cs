using System.Collections;
using System.Collections.Generic;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults
{
    public class CommentsQueryResult : IEnumerable<CommentDTO>, IQueryResult
    {
        private IEnumerable<CommentDTO> comments;

        public CommentsQueryResult(IEnumerable<CommentDTO> comments)
        {
            this.comments = comments;
        }
        public IEnumerator<CommentDTO> GetEnumerator()
        {
            return comments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
