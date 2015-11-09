using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.PlatformType;
using GameStore.BLL.QueryResults.PlatformType;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;
using EntryState = GameStore.Domain.Abstract.EntryState;

namespace GameStore.BLL.QueryHandlers.Platform
{
    public class GetAllPlatformTypesQueryHandler : IQueryHandler<GetAllPlatformTypesQuery, PlatformTypesQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetAllPlatformTypesQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
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
