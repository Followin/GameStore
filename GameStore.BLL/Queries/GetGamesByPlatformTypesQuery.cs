using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;
using GameStore.BLL.QueryResults;

namespace GameStore.BLL.Queries
{
    public class GetGamesByPlatformTypesQuery : IQuery
    {
        public Int32[] Ids { get; set; }
        public String[] Names { get; set; }
    }
}
