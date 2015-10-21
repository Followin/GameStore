using System;
using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class GameStoreUnitOfWork : IGameStoreUnitOfWork
    {
        private IContext db;
        private IRepository<Comment, Int32> comments;
        private IRepository<Game, Int32> games;
        private IRepository<Genre, Int32> genres;
        private IRepository<PlatformType, Int32> platformTypes;
        private IRepository<Publisher, Int32> publishers;
        private IRepository<OrderDetails, Int32> orderDetails;
        private IRepository<Order, Int32> orders; 

        public GameStoreUnitOfWork(IContext db)
        {
            this.db = db;
        }

        public IRepository<Comment, int> Comments
        {
            get { return comments ?? (comments = new GenericRepository<Comment>(db)); }
        }

        public IRepository<Game, int> Games
        {
            get { return games ?? (games = new GenericRepository<Game>(db)); }
        }

        public IRepository<Genre, int> Genres
        {
            get { return genres ?? (genres = new GenericRepository<Genre>(db)); }
        }

        public IRepository<PlatformType, int> PlatformTypes
        {
            get { return platformTypes ?? (platformTypes = new GenericRepository<PlatformType>(db)); }
        }

        public IRepository<Publisher, int> Publishers
        {
            get { return publishers ?? (publishers = new GenericRepository<Publisher>(db)); }
        }

        public IRepository<OrderDetails, int> OrderDetails
        {
            get { return orderDetails ?? (orderDetails = new GenericRepository<OrderDetails>(db)); }
        }

        public IRepository<Order, int> Orders
        {
            get { return orders ?? (orders = new GenericRepository<Order>(db)); }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
