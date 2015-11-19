using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults.Game;
using GameStore.BLL.Utils;
using GameStore.DAL.Abstract;
using NLog;

namespace GameStore.BLL.QueryHandlers.Games
{
    public class GetGameByKeyQueryHandler : IQueryHandler<GetGameByKeyQuery, GameQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetGameByKeyQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public GameQueryResult Retrieve(GetGameByKeyQuery query)
        {
            query.Key.Argument(NameGetter.GetName(() => query.Key))
                     .NotNull()
                     .NotWhiteSpace();
            var gameQueryResult = Mapper.Map<Domain.Entities.Game, GameQueryResult>(_db.Games.GetFirst(g => g.Key == query.Key));
            return gameQueryResult;
        }
    }
}
