using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults.Game;
using GameStore.DAL.Abstract;
using NLog;

namespace GameStore.BLL.QueryHandlers.Games
{
    public class GetGamesQueryHandler : IQueryHandler<GetGamesQuery, GamesPartQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetGamesQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public GamesPartQueryResult Retrieve(GetGamesQuery query)
        {
            var pipeline = new GameFilterPipeline(_db);
            var games = pipeline.Execute(query);

            return games;
        }
    }
}
