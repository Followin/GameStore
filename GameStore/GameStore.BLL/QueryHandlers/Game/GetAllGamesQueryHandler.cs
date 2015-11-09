using System.Collections.Generic;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults.Game;
using GameStore.Domain.Abstract;
using NLog;
using EntryState = GameStore.Domain.Abstract.EntryState;

namespace GameStore.BLL.QueryHandlers.Game
{
    public class GetAllGamesQueryHandler : IQueryHandler<GetAllGamesQuery, GamesQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetAllGamesQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public GamesQueryResult Retrieve(GetAllGamesQuery query)
        {
            return new GamesQueryResult(Mapper.Map<List<GameDTO>>(_db.Games.Get(g => g.EntryState == EntryState.Active)));
        }
    }
}
