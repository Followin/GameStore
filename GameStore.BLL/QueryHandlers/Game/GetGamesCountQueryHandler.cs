using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults;
using GameStore.Domain.Abstract;
using NLog;

namespace GameStore.BLL.QueryHandlers.Game
{
    public class GetGamesCountQueryHandler : IQueryHandler<GetGamesCountQuery, CountQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetGamesCountQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CountQueryResult Retrieve(GetGamesCountQuery query)
        {
            return new CountQueryResult(_db.Games.Get(x => x.EntryState == EntryState.Active).Count());
        }
    }
}
