using GameStore.DAL.Abstract;
using GameStore.DAL.Abstract.Repositories;
using GameStore.DAL.EF;

namespace GameStore.DAL.Repositories
{
    public class GameStoreUnitOfWork : IGameStoreUnitOfWork
    {
        private IEFContext _db;
        private INorthwindUnitOfWork _northwind;
        private ICommentRepository _comments;
        private IGameRepository _games;
        private IGenreRepository _genres;
        private IPlatformTypeRepository _platformTypes;
        private IPublisherRepository _publishers;
        private IOrderRepository _orders;
        private IUserRepository _users;


        public GameStoreUnitOfWork(IEFContext db, INorthwindUnitOfWork northwind)
        {
            _db = db;
            _northwind = northwind;
        }

        public ICommentRepository Comments
        {
            get { return _comments ?? (_comments = new CommentRepository(_db)); }
        }

        public IGameRepository Games
        {
            get { return _games ?? (_games = new GameRepository(_db, _northwind)); }
        }

        public IGenreRepository Genres
        {
            get { return _genres ?? (_genres = new GenreRepository(_db, _northwind)); }
        }

        public IPlatformTypeRepository PlatformTypes
        {
            get { return _platformTypes ?? (_platformTypes = new PlatformTypeRepository(_db)); }
        }

        public IPublisherRepository Publishers
        {
            get { return _publishers ?? (_publishers = new PublisherRepository(_db, _northwind)); }
        }


        public IOrderRepository Orders
        {
            get { return _orders ?? (_orders = new OrderRepository(_db, _northwind)); }
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
