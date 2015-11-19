using System.Collections.Generic;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults.Game;
using GameStore.BLL.Utils;
using GameStore.DAL.Abstract;
using NLog;

namespace GameStore.BLL.QueryHandlers.Games
{
    public class GetGamesByPublisherQueryHandler : 
        IQueryHandler<GetGamesByPublisherQuery, GamesQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetGamesByPublisherQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public GamesQueryResult Retrieve(GetGamesByPublisherQuery query)
        {
            query.Id.Argument(NameGetter.GetName(() => query.Id))
                    .GreaterThan(0);

            var games = _db.Games.Get(x => x.PublisherId == query.Id);
            var result = Mapper.Map<IEnumerable<Domain.Entities.Game>, IEnumerable<GameDTO>>(games);

            return new GamesQueryResult(result);
        }
    }
}
