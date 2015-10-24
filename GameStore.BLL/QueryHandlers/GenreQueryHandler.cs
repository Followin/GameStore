using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.Queries.Genre;
using GameStore.BLL.QueryResults;
using GameStore.BLL.QueryResults.Genre;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;
using EntryState = GameStore.Domain.Abstract.EntryState;

namespace GameStore.BLL.QueryHandlers
{
    public class GenreQueryHandler :
    #region interfaces
        IQueryHandler<GetAllGenresQuery, GenresQueryResult>
    #endregion
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GenreQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            this._logger = logger;
        }

        public GenresQueryResult Retrieve(GetAllGenresQuery query)
        {
            _logger.Debug("GetAllGenresQuery enter");
            return new GenresQueryResult(
                Mapper.Map<IEnumerable<Genre>, IEnumerable<GenreDTO>>(
                _db.Genres.Get(x => x.EntryState == EntryState.Active && x.ParentGenre == null)));
        }
    }
}
