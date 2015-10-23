using System;
using GameStore.BLL.CQRS;
using GameStore.BLL.QueryResults;

namespace GameStore.BLL.Queries
{
    public class GetCommentsByGameKeyQuery : IQuery
    {
        public String Key { get; set; }
    }
}
