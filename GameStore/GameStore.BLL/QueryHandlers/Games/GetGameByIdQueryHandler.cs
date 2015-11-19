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
    public class GetGameByIdQueryHandler : IQueryHandler<GetGameByIdQuery, GameQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetGameByIdQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public GameQueryResult Retrieve(GetGameByIdQuery query)
        {
            query.Id.Argument(NameGetter.GetName(() => query.Id))
                    .GreaterThan(0);
            return Mapper.Map<Domain.Entities.Game, GameQueryResult>(_db.Games.Get(query.Id));
        }
    }
}
