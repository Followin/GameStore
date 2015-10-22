using System;
using System.Collections.Generic;
using System.Linq;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries;
using GameStore.BLL.QueryResults;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;
using EntryState = GameStore.Domain.Abstract.EntryState;

namespace GameStore.BLL.QueryHandlers
{
    public class GameQueryHandler : 
        IQueryHandler<GetAllGamesQuery, GamesQueryResult>,
        IQueryHandler<GetGameByIdQuery, GameQueryResult>,
        IQueryHandler<GetGamesByGenreQuery, GamesQueryResult>,
        IQueryHandler<GetGamesByPlatformTypesQuery, GamesQueryResult>,
        IQueryHandler<GetGameByKeyQuery, GameQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GameQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public GamesQueryResult Retrieve(GetAllGamesQuery query)
        {
            return new GamesQueryResult(Mapper.Map<List<GameDTO>>(_db.Games.Get(g => g.EntryState == EntryState.Active)));
        }

        public GameQueryResult Retrieve(GetGameByIdQuery query)
        {
            query.Id.Argument("Id")
                    .GreaterThan(0);
            return Mapper.Map<Game, GameQueryResult>(_db.Games.Get(query.Id));
        }

        public GamesQueryResult Retrieve(GetGamesByGenreQuery query)
        {
            if (query.Id == 0 && query.Name == null)
            {
                throw new ArgumentException("Either Id or Name arguments must be specified", "Id, Name");
            }

            query.Name.Argument("Name")
                      .NotWhiteSpace();

            Genre genre;
            if (query.Id != 0)
            {
                query.Id.Argument("Id")
                    .GreaterThan(0);
                genre = _db.Genres.Get(query.Id);
                if (genre == null)
                {
                    throw new ArgumentOutOfRangeException("Id", "Genre not found");
                }
            }
            else
            {
                genre = _db.Genres.GetSingle(g => g.Name == query.Name);
                if (genre == null)
                {
                    throw new ArgumentException("Genre not found", "Name");
                }
            }

            var genres = genre.ChildGenres.ToList();
            genres.Add(genre);

            var games = _db.Games.Get(g => g.EntryState == EntryState.Active && g.Genres.Intersect(genres).Any());
            return new GamesQueryResult(Mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(games));
        }

        public GamesQueryResult Retrieve(GetGamesByPlatformTypesQuery query)
        {
            if (query.Ids == null && query.Names == null)
            {
                throw new ArgumentNullException("Ids, Names", "Either Ids or Names arguments must be specified");
            }

            IEnumerable<PlatformType> types;
            if (query.Ids != null)
            {
                query.Ids.Argument("Ids")
                         .NotEmpty();
                if (!query.Ids.All(x => x > 0))
                {
                    throw new ArgumentOutOfRangeException("Ids", "Ids must have only greater than zero numbers");
                }

                types = query.Ids.Select(id =>
                {
                    var type = _db.PlatformTypes.Get(id);
                    if (type == null)
                    {
                        throw new ArgumentOutOfRangeException(
                            "Ids", String.Format("Genre not found. Id: {0}", id));
                    }

                    return type;
                });
            }
            else
            {
                query.Names.Argument("Names")
                         .NotEmpty()
                         .AllMatch(
                              x => !String.IsNullOrWhiteSpace(x),
                              "Argument Names can't contain strings of white spaces");
                types = query.Names.Select(name =>
                {
                    var type = _db.PlatformTypes.GetSingle(pt => pt.Name == name);
                    if (type == null)
                    {
                        throw new ArgumentOutOfRangeException(
                            "Names", String.Format("Genre not found. Name: {0}", name));
                    }

                    return type;
                });
            }

            var games = _db.Games.Get(g => g.EntryState == EntryState.Active && types.Intersect(g.PlatformTypes).Any());
            return new GamesQueryResult(Mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(games));
        }

        public GameQueryResult Retrieve(GetGameByKeyQuery query)
        {
            query.Key.Argument("Key")
                     .NotNull()
                     .NotWhiteSpace();
           
            return Mapper.Map<GameQueryResult>(_db.Games.GetSingle(g => g.EntryState == EntryState.Active && g.Key == query.Key));
        }
    }
}
