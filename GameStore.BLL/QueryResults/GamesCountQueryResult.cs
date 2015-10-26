using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.QueryResults
{
    public class GamesCountQueryResult : IQueryResult
    {
        public GamesCountQueryResult(int count)
        {
            Count = count;
        }

        public Int32 Count { get; set; }
    }
}
