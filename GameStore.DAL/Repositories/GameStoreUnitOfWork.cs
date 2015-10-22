using System;
using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class GameStoreUnitOfWork : IGameStoreUnitOfWork
    {
        private IContext _db;
        private IRepository<Comment, Int32> _comments;
        private IRepository<Game, Int32> _games;
        private IRepository<Genre, Int32> _genres;
        private IRepository<PlatformType, Int32> _platformTypes;
        private IRepository<Publisher, Int32> _publishers;
        private IRepository<OrderDetails, Int32> _orderDetails;
        private IRepository<Order, Int32> _orders; 

        public GameStoreUnitOfWork(IContext db)
        {
            this._db = db;
        }

        public IRepository<Comment, int> Comments
        {
            get { return _comments ?? (_comments = new GenericRepository<Comment>(_db)); }
        }

        public IRepository<Game, int> Games
        {
            get { return _games ?? (_games = new GenericRepository<Game>(_db)); }
        }

        public IRepository<Genre, int> Genres
        {
            get { return _genres ?? (_genres = new GenericRepository<Genre>(_db)); }
        }

        public IRepository<PlatformType, int> PlatformTypes
        {
            get { return _platformTypes ?? (_platformTypes = new GenericRepository<PlatformType>(_db)); }
        }

        public IRepository<Publisher, int> Publishers
        {
            get { return _publishers ?? (_publishers = new GenericRepository<Publisher>(_db)); }
        }

        public IRepository<OrderDetails, int> OrderDetails
        {
            get { return _orderDetails ?? (_orderDetails = new GenericRepository<OrderDetails>(_db)); }
        }

        public IRepository<Order, int> Orders
        {
            get { return _orders ?? (_orders = new GenericRepository<Order>(_db)); }
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
