using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract.Repositories
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>Orders</returns>
        IEnumerable<Order> Get();

        /// <summary>
        /// Get order matching id
        /// </summary>
        /// <param name="id">Id to match</param>
        /// <returns>Order</returns>
        Order Get(Int32 id);

        /// <summary>
        /// Get orders matching predicate
        /// </summary>
        /// <param name="predicate">Predicate for order to match</param>
        /// <returns>Orders list</returns>
        IEnumerable<Order> Get(Func<Order, Boolean> predicate);
        
        /// <summary>
        /// Get's last unpayed order of the use. Creates one if doesn't exist
        /// </summary>
        /// <param name="userId">Current user</param>
        /// <returns>Order</returns>
        Order GetCurrentOrder(Int32 userId);

        /// <summary>
        /// Add order details
        /// </summary>
        /// <param name="orderDetails">Filled order details</param>
        void AddOrderDetails(OrderDetails orderDetails);

        /// <summary>
        /// Edit existing order details
        /// </summary>
        /// <param name="orderDetails">Filled order details</param>
        void EditOrderDetails(OrderDetails orderDetails);

        /// <summary>
        /// Get all shippers
        /// </summary>
        /// <returns>Shippers list</returns>
        IEnumerable<Shipper> GetShippers(); 

        /// <summary>
        /// Set order state to payment
        /// </summary>
        /// <param name="id">Order id</param>
        void Checkout(Int32 id);
    }
}