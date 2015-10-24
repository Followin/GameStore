using System;
using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class OrderRepository : GenericRepository<Order, Int32>, IOrderRepository
    {
        public OrderRepository(IContext context) : base(context)
        {
        }
    }
}
