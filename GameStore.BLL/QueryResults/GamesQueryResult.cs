﻿using System.Collections;
using System.Collections.Generic;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults
{
    public class GamesQueryResult : IEnumerable<GameDTO>, IQueryResult
    {
        private IEnumerable<GameDTO> _games;

        public GamesQueryResult(IEnumerable<GameDTO> games)
        {
            this._games = games;
        }

        public IEnumerator<GameDTO> GetEnumerator()
        {
            return _games.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
