using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Order;
using GameStore.BLL.QueryResults.Order;
using GameStore.DAL.Abstract;

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
            var orders = _db.Orders.Get();

            if (query.OnlyPaid)
            {
                orders = orders.Where(x => x.OrderDate.HasValue);
            }

            if (query.MinDate.HasValue)
            {
                orders = orders.Where(x => x.OrderDate.HasValue && x.OrderDate > query.MinDate.Value);
            }

            if (query.MaxDate.HasValue)
            {
                orders = orders.Where(x => x.OrderDate.HasValue && x.OrderDate < query.MaxDate.Value);
            }


            return new OrdersQueryResult(Mapper.Map<IEnumerable<Domain.Entities.Order>, IEnumerable<OrderQueryResult>>(orders));
        }
    }
}
