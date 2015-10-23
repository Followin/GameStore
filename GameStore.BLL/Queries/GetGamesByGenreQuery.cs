using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;
using GameStore.BLL.QueryResults;

namespace GameStore.BLL.Queries
{
    public class GetGamesByGenreQuery : IQuery
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }
    }
}
