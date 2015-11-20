using System;
using System.Collections.Generic;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults.Comment
{
    public class CommentQueryResult : IQueryResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Quotes { get; set; }

        public string Body { get; set; }

        public IEnumerable<CommentQueryResult> ChildComments { get; set; } 
    }
}
