using System;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract.Repositories
{
    public interface IOrderRepository : IRepository<Order, Int32>
    {
        
    }
}