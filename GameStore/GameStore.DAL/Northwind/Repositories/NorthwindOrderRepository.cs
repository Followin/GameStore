using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.DAL.Abstract;

namespace GameStore.DAL.Northwind.Repositories
{
    public class NorthwindOrderRepository : IOutRepository<Domain.Entities.Order>
    {
        private NorthwindContext _db;

        public NorthwindOrderRepository(NorthwindContext db)
        {
            _db = db;
        }

        private IEnumerable<Order> Orders
        {
            get { return _db.Orders; }
        } 

        public Domain.Entities.Order Get(int id)
        {
            return Mapper.Map<Order, Domain.Entities.Order>(Orders.FirstOrDefault(x => x.OrderID == id));
        }

        public IEnumerable<Domain.Entities.Order> Get()
        {
            return Mapper.Map<IEnumerable<Order>, IEnumerable<Domain.Entities.Order>>(Orders.ToList());
        }

        public IEnumerable<Domain.Entities.Order> GetExluding(IEnumerable<int> exludingIds)
        {
            return Mapper.Map<IEnumerable<Order>, IEnumerable<Domain.Entities.Order>>(Orders.Where(x => !exludingIds.Contains(x.OrderID)).ToList());
        }
    }
}
