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

        public GameStoreUnitOfWork(IContext db)
        {
            this._db = db;
        }

        public IRepository<Comment, int> Comments
        {
            get
            {
                return _comments ?? (_comments = new GenericRepository<Comment>(_db));
            }
        }

        public IRepository<Game, int> Games
        {
            get
            {
                return _games ?? (_games = new GenericRepository<Game>(_db));
            }
        }

        public IRepository<Genre, int> Genres
        {
            get
            {
                return _genres ?? (_genres = new GenericRepository<Genre>(_db));
            }
        }

        public IRepository<PlatformType, int> PlatformTypes
        {
            get
            {
                return _platformTypes ?? (_platformTypes = new GenericRepository<PlatformType>(_db));
            }
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
