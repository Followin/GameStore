using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using GameStore.DAL.Abstract;
using GameStore.DAL.EF;
using GameStore.DAL.Static;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private EFContext _db;
        private INorthwindUnitOfWork _northwind;

        public OrderRepository(EFContext db, INorthwindUnitOfWork northwind)
        {
            _db = db;
            _northwind = northwind;
        }

        private void FillOrder(Order order)
        {
            if (order.OrderDetails == null || !order.OrderDetails.Any()) return;
            foreach (var detail in order.OrderDetails)
            {
                var database = KeyEncoder.GetBase(detail.GameId);
                switch (database)
                {
                    case DatabaseTypes.GameStore:
                        detail.Game = _db.Games.Find(detail.GameId);
                        break;
                    case DatabaseTypes.Northwind:
                        detail.Game = _northwind.Games.Get(KeyEncoder.GetId(detail.GameId));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public IEnumerable<Order> Get()
        {
            var orders = _db.Orders.ToList();
            orders.ForEach(FillOrder);
            return orders.Concat(_northwind.Orders.Get());
        }

        public Order Get(int id)
        {
            var databaseType = KeyEncoder.GetBase(id);
            switch (databaseType)
            {
                case DatabaseTypes.GameStore:
                    var order = _db.Orders.Find(id);
                    FillOrder(order);
                    return order;
                case DatabaseTypes.Northwind:
                    order = _northwind.Orders.Get(KeyEncoder.GetId(id));
                    return order;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public IEnumerable<Order> Get(Func<Order, bool> predicate)
        {
            return Get().Where(predicate);
        }

        public Order GetCurrentOrder(int userId)
        {
            var existingOrder = _db.Orders.FirstOrDefault(x => x.UserId == userId && !x.OrderDate.HasValue);
            if (existingOrder == null)
            {
                Int32 nextId = KeyEncoder.Coefficient + (Int32)DatabaseTypes.GameStore;
                if (_db.Orders.Any())
                {
                    nextId = _db.Orders.Max(x => x.Id) + KeyEncoder.Coefficient;
                }
                existingOrder = new Order { Id = nextId, UserId = userId, OrderDetails = new List<OrderDetails>() };
                _db.Orders.Add(existingOrder);

                _db.SaveChanges();
            }
            FillOrder(existingOrder);
            return existingOrder;
        }

        public void AddOrderDetails(OrderDetails orderDetails)
        {
            _db.OrderDetails.Add(orderDetails);
        }

        public void EditOrderDetails(OrderDetails orderDetails)
        {
            _db.Entry(orderDetails).State = EntityState.Modified;
        }

        public IEnumerable<Shipper> GetShippers()
        {
            return _northwind.GetShippers.ToList();
        }

        public void Checkout(int id)
        {
            var order = _db.Orders.Find(id);
            order.OrderDate = DateTime.UtcNow;
            _db.Entry(order).State = EntityState.Modified;
        }

        public DateTime Ship(int id)
        {
            var now = DateTime.UtcNow;
            var order = _db.Orders.Find(id);

            order.ShippedDate = DateTime.UtcNow;
            _db.Entry(order).State = EntityState.Modified;

            return now;
        }
    }
}
