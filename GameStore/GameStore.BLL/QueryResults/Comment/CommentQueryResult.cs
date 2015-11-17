using System;
using System.Collections.Generic;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults.Comment
{
    public class CommentQueryResult : IQueryResult
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }

        public String Quotes { get; set; }

        public String Body { get; set; }

        public IEnumerable<CommentQueryResult> ChildComments { get; set; } 
    }
}
