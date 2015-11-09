using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Comment
{
    public class GetCommentsByGameKeyQuery : IQuery
    {
        public String Key { get; set; }
    }
}
