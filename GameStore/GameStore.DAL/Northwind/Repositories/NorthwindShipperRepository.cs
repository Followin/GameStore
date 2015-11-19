using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.DAL.Abstract.Repositories;
using StoreShipper = GameStore.Domain.Entities.Shipper;

namespace GameStore.DAL.Northwind.Repositories
{
    public class NorthwindShipperRepository : IOutRepository<StoreShipper>
    {
        private NorthwindContext _db;

        public NorthwindShipperRepository(NorthwindContext db)
        {
            _db = db;
        }

        public StoreShipper Get(int id)
        {
            return Mapper.Map<StoreShipper>(_db.Shippers.FirstOrDefault(x => x.ShipperID == id));
        }

        public IEnumerable<StoreShipper> Get()
        {
            return Mapper.Map<IEnumerable<StoreShipper>>(_db.Shippers.ToList());
        }

        public IEnumerable<StoreShipper> GetExluding(IEnumerable<int> exludingIds)
        {
            return Mapper.Map<IEnumerable<StoreShipper>>(_db.Shippers.Where(x => !exludingIds.Contains(x.ShipperID)).ToList());
        }

        public IEnumerable<StoreShipper> GetIncluding(IEnumerable<int> includingIds)
        {
            return Mapper.Map<IEnumerable<StoreShipper>>(_db.Shippers.Where(x => includingIds.Contains(x.ShipperID)).ToList());
        }
    }
}
