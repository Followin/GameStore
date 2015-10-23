using System;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract.Repositories
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails, Int32>
    {

    }
}