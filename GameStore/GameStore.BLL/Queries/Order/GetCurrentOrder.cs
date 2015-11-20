using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Order
{
    public class GetCurrentOrderQuery : IQuery
    {
        public int UserId { get; set; }
    }
}
