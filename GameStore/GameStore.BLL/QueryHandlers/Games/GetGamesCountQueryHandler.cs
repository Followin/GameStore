using System.Linq;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults;
using GameStore.DAL.Abstract;
using GameStore.Domain.Entities;
using GameStore.Static;
using NLog;

namespace GameStore.BLL.QueryHandlers.Games
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
