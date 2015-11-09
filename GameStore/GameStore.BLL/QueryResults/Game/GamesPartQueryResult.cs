using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults.Game
{
    public class GamesPartQueryResult : GamesQueryResult
    {
        public Int32 Count { get; private set; }

        public GamesPartQueryResult(IEnumerable<GameDTO> games, int count) : base(games)
        {
            Count = count;
        }
    }
}
