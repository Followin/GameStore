using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Publisher;
using GameStore.BLL.QueryResults.Genre;
using GameStore.BLL.QueryResults.Publisher;
using GameStore.DAL.Abstract;
using NLog;

namespace GameStore.BLL.QueryHandlers.Publisher
{
    public class GetPublisherByIdQueryHandler :
        IQueryHandler<GetPublisherByIdQuery, PublisherQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetPublisherByIdQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public PublisherQueryResult Retrieve(GetPublisherByIdQuery query)
        {
            return Mapper.Map<PublisherQueryResult>(_db.Publishers.Get(query.Id));
        }
    }
}
