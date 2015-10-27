using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Game
{
    public class IsGameVisitedByUserQuery : IQuery
    {
        public Int32 GameId { get; set; }

        public Int32 UserId { get; set; }
    }
}
