using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using GameStore.DAL.Abstract;
using GameStore.DAL.Abstract.Repositories;
using GameStore.DAL.EF;
using GameStore.DAL.Static;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private IEFContext _db;
        private INorthwindUnitOfWork _northwind;

        public OrderRepository(IEFContext db, INorthwindUnitOfWork northwind)
        {
            _db = db;
            _northwind = northwind;
        }

        private void FillOrder(Order order)
        {
            if (order.OrderDetails == null || !order.OrderDetails.Any())
            {
                return;
            }

            var gameIds = order.OrderDetails
                               .Select(x => x.GameId)
                               .GroupBy(KeyEncoder.GetBase)
                               .ToList();
            var mainGameIds = gameIds.FirstOrDefault(x => x.Key == DatabaseTypes.GameStore);
            var northwindGameIds = gameIds.FirstOrDefault(x => x.Key == DatabaseTypes.Northwind);

            List<Game> games = new List<Game>();

            if (mainGameIds != null)
            {
                games.AddRange(_db.Games.Where(x => mainGameIds.Contains(x.Id)));
            }
            if (northwindGameIds != null)
            {
                games.AddRange(_northwind.Games.GetIncluding(northwindGameIds.Select(KeyEncoder.GetId)));
            }

            foreach (var detail in order.OrderDetails)
            {
                detail.Game = games.Find(x => x.Id == detail.GameId);
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
            Order order = null;
            switch (databaseType)
            {
                case DatabaseTypes.GameStore:
                    order = _db.Orders.Find(id);
                    FillOrder(order);
                    break;
                case DatabaseTypes.Northwind:
                    order = _northwind.Orders.Get(KeyEncoder.GetId(id));
                    break;
            }

            return order;

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
                Int32 nextId = KeyEncoder.GetNext(DatabaseTypes.GameStore);
                if (_db.Orders.Any())
                {
                    nextId = KeyEncoder.GetNext(_db.Orders.Max(x => x.Id));
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
            _db.SetModified(orderDetails);
        }

        public void DeleteOrderDetails(Int32 gameId, Int32 orderId)
        {
            var orderDetails = _db.OrderDetails.Find(gameId, orderId);
            if (orderDetails != null)
            {
                _db.OrderDetails.Remove(orderDetails);
            }
        }

        public IEnumerable<Shipper> GetShippers()
        {
            return _northwind.Shippers.Get();
        }

        public void Checkout(int id)
        {
            var order = _db.Orders.Find(id);
            order.OrderDate = DateTime.UtcNow;
            _db.SetModified(order);
        }

        public DateTime Ship(int id)
        {
            var now = DateTime.UtcNow;
            var order = _db.Orders.Find(id);

            order.ShippedDate = DateTime.UtcNow;
            _db.SetModified(order);

            return now;
        }
    }
}
