using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults.Game;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;
using EntryState = GameStore.Domain.Abstract.EntryState;

namespace GameStore.BLL.QueryHandlers.Game
{
    public class GetGamesByGenreQueryHandler : IQueryHandler<GetGamesByGenreQuery, GamesQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetGamesByGenreQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public GamesQueryResult Retrieve(GetGamesByGenreQuery query)
        {
            if (query.Id == 0 && query.Name == null)
            {
                throw new ArgumentException(
                    "Either Id or Name arguments must be specified",
                    NameGetter.GetName(() => query.Id) + ", " + NameGetter.GetName(() => query.Name));
            }

            query.Name.Argument(NameGetter.GetName(() => query.Name))
                      .NotWhiteSpace();

            Domain.Entities.Genre genre;
            if (query.Id != 0)
            {
                query.Id.Argument("Id")
                    .GreaterThan(0);
                genre = _db.Genres.Get(query.Id);
                if (genre == null)
                {
                    throw new ArgumentOutOfRangeException(
                        NameGetter.GetName(() => query.Id),
                        "Genre not found");
                }
            }
            else
            {
                genre = _db.Genres.GetSingle(g => g.NameEn == query.Name);
                if (genre == null)
                {
                    throw new EntityNotFoundException(
                        "Genre not found",
                        NameGetter.GetName(() => query.Name));
                }
            }

            var genres = genre.ChildGenres.ToList();
            genres.Add(genre);

            var games = _db.Games.Get(g => g.EntryState == EntryState.Active && g.Genres.Intersect(genres).Any());
            return new GamesQueryResult(Mapper.Map<IEnumerable<Domain.Entities.Game>, IEnumerable<GameDTO>>(games));
        }
    }
}
