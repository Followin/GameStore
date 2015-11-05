using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> Get();

        Order Get(Int32 id);

        IEnumerable<Order> Get(Func<Order, Boolean> predicate);
        
        Order GetCurrentOrder(Int32 userId);

        void AddOrderDetails(OrderDetails orderDetails);

        void EditOrderDetails(OrderDetails orderDetails);

        IEnumerable<Shipper> GetShippers(); 

        void Checkout(Int32 id);
    }
}