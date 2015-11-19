using System;
using System.Collections.Generic;
using System.Linq;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults.Game;
using GameStore.BLL.Utils;
using GameStore.DAL.Abstract;
using GameStore.Domain.Entities;
using NLog;
using EntryState = GameStore.Domain.Entities.EntryState;

namespace GameStore.BLL.QueryHandlers.Games
{
    public class GetGamesByPlatformTypesQueryHandler : IQueryHandler<GetGamesByPlatformTypesQuery, GamesQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetGamesByPlatformTypesQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public GamesQueryResult Retrieve(GetGamesByPlatformTypesQuery query)
        {
            if (query.Ids == null && query.Names == null)
            {
                throw new ArgumentNullException(
                    NameGetter.GetName(() => query.Ids) + ", " + NameGetter.GetName(() => query.Names),
                    "Either Ids or Names arguments must be specified");
            }

            IEnumerable<PlatformType> types;
            if (query.Ids != null)
            {
                query.Ids.Argument("Ids")
                         .NotEmpty();
                if (!query.Ids.All(x => x > 0))
                {
                    throw new ArgumentOutOfRangeException(
                        NameGetter.GetName(() => query.Ids),
                        "Ids must have only greater than zero numbers");
                }

                types = query.Ids.Select(id =>
                {
                    var type = _db.PlatformTypes.Get(id);
                    if (type == null)
                    {
                        throw new ArgumentOutOfRangeException(
                            NameGetter.GetName(() => query.Ids),
                            String.Format("Genre not found. Id: {0}", id));
                    }

                    return type;
                });
            }
            else
            {
                query.Names.Argument(NameGetter.GetName(() => query.Names))
                         .NotEmpty()
                         .AllMatch(
                              x => !String.IsNullOrWhiteSpace(x),
                              "Argument Names can't contain strings of white spaces");
                types = query.Names.Select(name =>
                {
                    var type = _db.PlatformTypes.GetFirst(pt => pt.Name == name);
                    if (type == null)
                    {
                        throw new ArgumentOutOfRangeException(
                            NameGetter.GetName(() => query.Names),
                            String.Format("Genre not found. Name: {0}", name));
                    }

                    return type;
                });
            }

            var games = _db.Games.Get(g => g.EntryState == EntryState.Active && types.Intersect(g.PlatformTypes).Any());
            return new GamesQueryResult(Mapper.Map<IEnumerable<Domain.Entities.Game>, IEnumerable<GameDTO>>(games));
        }
    }
}
