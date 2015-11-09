using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.User
{
    public class GetUserBySessionIdQuery : IQuery
    {
        public String SessionId { get; set; }
    }
}
