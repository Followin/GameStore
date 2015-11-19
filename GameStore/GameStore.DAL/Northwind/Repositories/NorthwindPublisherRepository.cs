using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.DAL.Abstract;
using GameStore.DAL.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Northwind.Repositories
{
    public class NorthwindPublisherRepository : IOutRepository<Publisher>
    {
        private NorthwindContext _db;

        public NorthwindPublisherRepository(NorthwindContext db)
        {
            _db = db;
        }

        private IEnumerable<Supplier> Suppliers
        {
            get { return _db.Suppliers.AsNoTracking(); }
        }

        public Publisher Get(int id)
        {
            return Mapper.Map<Supplier, Publisher>(Suppliers.FirstOrDefault());
        }

        public IEnumerable<Publisher> Get()
        {
            return Mapper.Map<IEnumerable<Supplier>, IEnumerable<Publisher>>(Suppliers.ToList());
        }

        public IEnumerable<Publisher> GetExluding(IEnumerable<int> exludingIds)
        {
            return Mapper.Map<IEnumerable<Supplier>, IEnumerable<Publisher>>(Suppliers.Where(x => !exludingIds.Contains(x.SupplierID)).ToList());
        }

        public IEnumerable<Publisher> GetIncluding(IEnumerable<int> includingIds)
        {
            return Mapper.Map<IEnumerable<Supplier>, IEnumerable<Publisher>>(Suppliers.Where(x => includingIds.Contains(x.SupplierID)).ToList());
        }
    }
}
