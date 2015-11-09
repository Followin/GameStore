using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Order;
using GameStore.BLL.QueryResults.Order;
using GameStore.Domain.Abstract;

namespace GameStore.BLL.QueryHandlers.Order
{
    public class GetOrdersHistoryQueryHandler : IQueryHandler<GetOrdersHistoryQuery, OrdersQueryResult>
    {
        private IGameStoreUnitOfWork _db;

        public GetOrdersHistoryQueryHandler(IGameStoreUnitOfWork db)
        {
            _db = db;
        }

        public OrdersQueryResult Retrieve(GetOrdersHistoryQuery query)
        {
            var orders = _db.Orders.Get().ToList();
            return new OrdersQueryResult(Mapper.Map<IEnumerable<Domain.Entities.Order>, IEnumerable<OrderQueryResult>>(orders));
        }
    }
}
