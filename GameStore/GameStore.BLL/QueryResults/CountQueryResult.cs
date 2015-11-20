using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.QueryResults
{
    public class CountQueryResult : IQueryResult
    {
        public CountQueryResult(int count)
        {
            Count = count;
        }

        public int Count { get; set; }
    }
}
