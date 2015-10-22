using System.Collections.Generic;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.PlatformType;
using GameStore.BLL.QueryResults;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;
using EntryState = GameStore.Domain.Abstract.EntryState;

namespace GameStore.BLL.QueryHandlers
{
    public class PlatformTypeQueryHandler :
    #region interfaces
        IQueryHandler<GetAllPlatformTypesQuery, PlatformTypesQueryResult>
    #endregion

    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public PlatformTypeQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public PlatformTypesQueryResult Retrieve(GetAllPlatformTypesQuery query)
        {
            _logger.Debug("GetAllPlatformTypes enter");
            return new PlatformTypesQueryResult(
                Mapper.Map<IEnumerable<PlatformType>, IEnumerable<PlatformTypeDTO>>(
                    _db.PlatformTypes.Get(x => x.EntryState == EntryState.Active)));
        }
    }
}
