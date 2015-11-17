using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Comment
{
    public class GetCommentsForGameQuery : IQuery
    {
        public String Key { get; set; }

        public Int32 Id { get; set; }
    }
}
