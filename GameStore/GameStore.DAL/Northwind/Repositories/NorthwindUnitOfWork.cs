using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.DAL.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Northwind.Repositories
{
    public class NorthwindUnitOfWork : INorthwindUnitOfWork
    {
        private NorthwindContext _db;
        private IOutRepository<Game> _games;
        private IOutRepository<Publisher> _publishers;
        private IOutRepository<Genre> _genres;
        private IOutRepository<Domain.Entities.Order> _orders;

        public NorthwindUnitOfWork(NorthwindContext db)
        {
            _db = db;
        }

        public IOutRepository<Game> Games
        {
            get { return _games ?? (_games = new NorthwindGameRepository(_db)); }
        }

        public IOutRepository<Publisher> Publishers
        {
            get { return _publishers ?? (_publishers = new NorthwindPublisherRepository(_db)); }
        }

        public IOutRepository<Domain.Entities.Order> Orders
        {
            get { return _orders ?? (_orders = new NorthwindOrderRepository(_db)); }
        }


        public IOutRepository<Genre> Genres
        {
            get { return _genres ?? (_genres = new NorthwindGenreRepository(_db)); }
        }

        public IEnumerable<Domain.Entities.Shipper> GetShippers
        {
            get
            {
                return Mapper.Map<IEnumerable<Shipper>, IEnumerable<Domain.Entities.Shipper>>(_db.Shippers.ToList());
            }
        }
    }
}
