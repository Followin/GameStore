using System;
using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class GameStoreUnitOfWork : IGameStoreUnitOfWork
    {
        private IContext _db;
        private ICommentRepository _comments;
        private IGameRepository _games;
        private IGenreRepository _genres;
        private IPlatformTypeRepository _platformTypes;
        private IPublisherRepository _publishers;
        private IOrderDetailsRepository _orderDetails;
        private IOrderRepository _orders;
        private IUserRepository _users;

        public GameStoreUnitOfWork(IContext db, ICommentRepository comments, IGameRepository games, IGenreRepository genres, IPlatformTypeRepository platformTypes, IPublisherRepository publishers, IOrderDetailsRepository orderDetails, IOrderRepository orders, IUserRepository users)
        {
            _db = db;
            _comments = comments;
            _games = games;
            _genres = genres;
            _platformTypes = platformTypes;
            _publishers = publishers;
            _orderDetails = orderDetails;
            _orders = orders;
            _users = users;
        }

        public GameStoreUnitOfWork(IContext db)
        {
            this._db = db;
        }

        public ICommentRepository Comments
        {
            get { return _comments ?? (_comments = new CommentRepository(_db)); }
        }

        public IGameRepository Games
        {
            get { return _games ?? (_games = new GameRepository(_db)); }
        }

        public IGenreRepository Genres
        {
            get { return _genres ?? (_genres = new GenreRepository(_db)); }
        }

        public IPlatformTypeRepository PlatformTypes
        {
            get { return _platformTypes ?? (_platformTypes = new PlatformTypeRepository(_db)); }
        }

        public IPublisherRepository Publishers
        {
            get { return _publishers ?? (_publishers = new PublisherRepository(_db)); }
        }

        public IOrderDetailsRepository OrderDetails
        {
            get { return _orderDetails ?? (_orderDetails = new OrderDetailsRepository(_db)); }
        }

        public IOrderRepository Orders
        {
            get { return _orders ?? (_orders = new OrderRepository(_db)); }
        }

        public IUserRepository Users
        {
            get { return _users ?? (_users = new UserRepository(_db)); }
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
