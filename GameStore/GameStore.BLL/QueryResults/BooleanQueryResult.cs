using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.QueryResults
{
    public class BooleanQueryResult : IQueryResult
    {
        public bool Result { get; set; }

        public static implicit operator bool(BooleanQueryResult result)
        {
            return result.Result;
        }

        public static implicit operator BooleanQueryResult(bool @value)
        {
            return new BooleanQueryResult {Result = @value};
        }
    }
}
