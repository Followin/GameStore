using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.DAL.Abstract;
using GameStore.DAL.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Northwind.Repositories
{
    public class NorthwindGameRepository : IOutRepository<Game>
    {
        private NorthwindContext _db;

        public NorthwindGameRepository(NorthwindContext db)
        {
            _db = db;
        }

        private IEnumerable<Product> Products
        {
            get { return _db.Products; }
        } 

        public Game Get(int id)
        {
            return Mapper.Map<Product, Game>(Products.FirstOrDefault(x => x.ProductID == id));
        }

        public IEnumerable<Game> Get()
        {
            return Mapper.Map<IEnumerable<Product>, IEnumerable<Game>>(Products.ToList());
        }

        public IEnumerable<Game> GetExluding(IEnumerable<int> exludingIds)
        {
            return Mapper.Map<IEnumerable<Product>, IEnumerable<Game>>(Products.Where(x => !exludingIds.Contains(x.ProductID)).ToList());
        }

        public IEnumerable<Game> GetIncluding(IEnumerable<int> includingIds)
        {
            return Mapper.Map<IEnumerable<Product>, IEnumerable<Game>>(Products.Where(x => includingIds.Contains(x.ProductID)).ToList());
        }
    }
}
