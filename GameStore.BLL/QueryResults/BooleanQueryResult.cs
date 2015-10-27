using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.QueryResults
{
    public class BooleanQueryResult : IQueryResult
    {
        public Boolean Result { get; set; }

        public static implicit operator Boolean(BooleanQueryResult result)
        {
            return result.Result;
        }

        public static implicit operator BooleanQueryResult(Boolean @value)
        {
            return new BooleanQueryResult {Result = @value};
        }
    }
}
