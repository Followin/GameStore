using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Genre;
using GameStore.BLL.QueryResults.Genre;
using GameStore.DAL.Abstract;
using NLog;

namespace GameStore.BLL.QueryHandlers.Genre
{
    public class GetGenreByIdQueryHandler :
        IQueryHandler<GetGenreByIdQuery, GenreQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetGenreByIdQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public GenreQueryResult Retrieve(GetGenreByIdQuery query)
        {
            return Mapper.Map<GenreQueryResult>(_db.Genres.Get(query.Id));
        }
    }
}
