using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Abstract;
using GameStore.DAL.EF;
using GameStore.Domain.Entities;
using GameStore.Domain.Abstract;
using GameStore.Domain.Abstract.Entities;

namespace GameStore.DAL.Repositories
{
    public class GameStoreUnitOfWork : IGameStoreUnitOfWork
    {
        private IContext db;
        private IRepository<Comment, Int32> comments;
        private IRepository<Game, Int32> games;
        private IRepository<Genre, Int32> genres;
        private IRepository<PlatformType, Int32> platformTypes;

        public GameStoreUnitOfWork(IContext db)
        {
            this.db = db;
        }
        public IRepository<Comment, int> Comments
        {
            get
            {
                return comments ?? (comments = new GenericRepository<Comment>(db));
            }
        }

        public IRepository<Game, int> Games
        {
            get
            {
                return games ?? (games = new GenericRepository<Game>(db));
            }
        }

        public IRepository<Genre, int> Genres
        {
            get
            {
                return genres ?? (genres = new GenericRepository<Genre>(db));
            }
        }


        public IRepository<PlatformType, int> PlatformTypes
        {
            get
            {
                return platformTypes ?? (platformTypes = new GenericRepository<PlatformType>(db));
            }
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
