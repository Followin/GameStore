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
using GameStore.DAL.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.BLL.QueryHandlers.Order
{
    public class GetShippersQueryHandler : IQueryHandler<GetShippersQuery, ShippersQueryResult>
    {
        private IGameStoreUnitOfWork _db;

        public GetShippersQueryHandler(IGameStoreUnitOfWork db)
        {
            _db = db;
        }

        public ShippersQueryResult Retrieve(GetShippersQuery query)
        {
            return new ShippersQueryResult(Mapper.Map<IEnumerable<Shipper>, IEnumerable<ShipperDTO>>(_db.Orders.GetShippers()));
        }
    }
}
