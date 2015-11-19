using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.Publisher;
using GameStore.BLL.QueryResults.Publisher;
using GameStore.DAL.Abstract;
using NLog;
using EntryState = GameStore.Domain.Entities.EntryState;

namespace GameStore.BLL.QueryHandlers.Publisher
{
    public class GetAllPublishersQueryhandler : IQueryHandler<GetAllPublishersQuery, PublishersQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetAllPublishersQueryhandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public PublishersQueryResult Retrieve(GetAllPublishersQuery query)
        {
            _logger.Debug("GetAllPublishers enter");
            return new PublishersQueryResult(
                          Mapper.Map<IEnumerable<Domain.Entities.Publisher>, IEnumerable<PublisherDTO>>(
                              _db.Publishers.Get(p => p.EntryState == EntryState.Active)));
        }
    }
}
