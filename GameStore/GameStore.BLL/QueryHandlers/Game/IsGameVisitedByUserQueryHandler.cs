using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using NLog;

namespace GameStore.BLL.QueryHandlers.Game
{
    public class IsGameVisitedByUserQueryHandler : IQueryHandler<IsGameVisitedByUserQuery, BooleanQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public IsGameVisitedByUserQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public BooleanQueryResult Retrieve(IsGameVisitedByUserQuery query)
        {
            var game = _db.Games.Get(query.GameId);
            if (game == null)
                throw new ArgumentOutOfRangeException(
                    NameGetter.GetName(() => query.GameId),
                    "Game not found");

            return game.UsersViewed != null && game.UsersViewed.Any(x => x.Id == query.UserId);
        }
    }
}
