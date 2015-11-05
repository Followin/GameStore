using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Abstract;
using GameStore.DAL.References;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF.Repositories
{
    public class MainGameRepository : IGameRepository
    {
        private IContext _db;
        private IReferenceManager<Game> _gamesManager;

        public MainGameRepository(IContext db, IReferenceManager<Game> gamesManager)
        {
            _db = db;
            _gamesManager = gamesManager;
        }

        private IEnumerable<Game> Games
        {
            get { return _db.Games.AsNoTracking().Include(g => g.PlatformTypes); }
        } 

        public Game Get(int id)
        {
            var game = Games.FirstOrDefault(x => x.Id == id);

            return _gamesManager.Sync(game, DatabaseNames.GameStore);
        }

        public IEnumerable<Game> Get()
        {
            var games = Games.ToList();

            return _gamesManager.Sync(games, DatabaseNames.GameStore);
        }

        public IEnumerable<Game> Get(Func<Game, bool> predicate)
        {
            var games = Games.Where(predicate).ToList();

            return _gamesManager.Sync(games, DatabaseNames.GameStore);
        }

        public Game GetSingle(Func<Game, bool> predicate)
        {
            var game = Games.FirstOrDefault(predicate);

            return _gamesManager.Sync(game, DatabaseNames.GameStore);
        }

        public void Add(Game item)
        {
            _db.Games.Add(item);
        }

        public void Update(Game item)
        {
            _db.SetModified(item);
        }

        public void Delete(int id)
        {
            var game = _db.Games.Find(id);
            if (game != null)
            {
                _db.Games.Remove(game);
            }
        }

        public int GetCount(Expression<Func<Game, bool>> predicate = null)
        {
            return predicate == null ? _db.Games.Count() : _db.Games.Count(predicate);
        }

        public IEnumerable<Game> Get(Expression<Func<Game, bool>> predicate, string comparer, int? skip = null, int? number = null)
        {
            throw new NotImplementedException();
        }
    }
}
