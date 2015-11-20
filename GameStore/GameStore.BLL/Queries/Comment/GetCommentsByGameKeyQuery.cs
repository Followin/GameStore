using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Comment
{
    public class GetCommentsForGameQuery : IQuery
    {
        public string Key { get; set; }

        public int Id { get; set; }
    }
}
