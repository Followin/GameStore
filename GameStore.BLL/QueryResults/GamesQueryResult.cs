using System.Collections;
using System.Collections.Generic;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults
{
    public class GamesQueryResult : IEnumerable<GameDTO>, IQueryResult
    {
        private IEnumerable<GameDTO> games;

        public GamesQueryResult(IEnumerable<GameDTO> games)
        {
            this.games = games;
        }


        public IEnumerator<GameDTO> GetEnumerator()
        {
            return games.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
