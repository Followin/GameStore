using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Order;
using GameStore.BLL.QueryResults.Order;
using GameStore.Domain.Abstract;
using NLog;

namespace GameStore.BLL.QueryHandlers.Order
{
    public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetOrderByIdQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public OrderQueryResult Retrieve(GetOrderByIdQuery query)
        {
            return Mapper.Map<Domain.Entities.Order, OrderQueryResult>(_db.Orders.Get(query.Id));
        }
    }
}
