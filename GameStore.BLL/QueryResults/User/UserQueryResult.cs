using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.QueryResults.User
{
    public class UserQueryResult : IQueryResult
    {
        public Int32 Id { get; set; }

        public String SessionId { get; set; }
    }
}
