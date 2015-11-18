using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Order;
using GameStore.BLL.QueryResults.Order;
using GameStore.Domain.Abstract;

namespace GameStore.BLL.QueryHandlers.Order
{
    public class GetCurrentOrderQueryHandler : IQueryHandler<GetCurrentOrderQuery, OrderQueryResult>
    {
        private IGameStoreUnitOfWork _db;

        public GetCurrentOrderQueryHandler(IGameStoreUnitOfWork db)
        {
            _db = db;
        }

        public OrderQueryResult Retrieve(GetCurrentOrderQuery query)
        {
            var mappedOrder = Mapper.Map<Domain.Entities.Order, OrderQueryResult>(_db.Orders.GetCurrentOrder(query.UserId));
            return mappedOrder;
        }
    }
}
