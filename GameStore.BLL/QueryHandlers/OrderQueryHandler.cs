using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.Order;
using GameStore.BLL.QueryResults.Order;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.BLL.QueryHandlers
{
    public class OrderQueryHandler :
    #region interfaces
        IQueryHandler<GetCurrentOrder, OrderQueryResult>,
        IQueryHandler<GetOrdersHistoryQuery, OrdersQueryResult>,
        IQueryHandler<GetShippersQuery, ShippersQueryResult>
    #endregion
    {
        private IGameStoreUnitOfWork _db;

        public OrderQueryHandler(IGameStoreUnitOfWork db)
        {
            _db = db;
        }

        public OrderQueryResult Retrieve(GetCurrentOrder query)
        {
            var mappedOrder = Mapper.Map<Order, OrderQueryResult>(_db.Orders.GetCurrentOrder(query.UserId));
            return mappedOrder;
        }

        public OrdersQueryResult Retrieve(GetOrdersHistoryQuery query)
        {
            var orders = _db.Orders.Get().ToList();
            return new OrdersQueryResult(Mapper.Map<IEnumerable<Order>, IEnumerable<OrderQueryResult>>(orders));
        }

        public ShippersQueryResult Retrieve(GetShippersQuery query)
        {
            return new ShippersQueryResult(Mapper.Map<IEnumerable<Shipper>, IEnumerable<ShipperDTO>>(_db.Orders.GetShippers()));
        }
    }
}
