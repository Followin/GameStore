using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF.Repositories
{
    public class MainOrderRepository : IOrderRepository
    {
        public Order Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> Get(Func<Order, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Order GetSingle(Func<Order, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Add(Order item)
        {
            throw new NotImplementedException();
        }

        public void Update(Order item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<Order, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }
    }
}
