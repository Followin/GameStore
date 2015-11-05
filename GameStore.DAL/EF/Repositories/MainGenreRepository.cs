using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Abstract;
using GameStore.DAL.References;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF.Repositories
{
    public class MainGenreRepository : IGenreRepository
    {
        private IContext _db;
        private IReferenceManager<Genre> _genresManager;
        private IDbSet<Genre> _set; 

        public MainGenreRepository(IContext db, IReferenceManager<Genre> genresManager)
        {
            _db = db;
            _genresManager = genresManager;
        }


        public Genre Get(int id)
        {
            var genre = _db.Genres.AsNoTracking().FirstOrDefault(x => x.Id == id);

            return _genresManager.Sync(genre, DatabaseNames.GameStore);  
        }

        public IEnumerable<Genre> Get()
        {
            var genres = _db.Genres.AsNoTracking().ToList();

            return _genresManager.Sync(genres, DatabaseNames.GameStore);
        }

        public IEnumerable<Genre> Get(Func<Genre, bool> predicate)
        {
            var genres = _db.Genres.AsNoTracking().Where(predicate).ToList();

            return _genresManager.Sync(genres, DatabaseNames.GameStore);
        }

        public Genre GetSingle(Func<Genre, bool> predicate)
        {
            var genre = _db.Genres.AsNoTracking().FirstOrDefault(predicate);

            return _genresManager.Sync(genre, DatabaseNames.GameStore);
        }

        public void Add(Genre item)
        {
            _db.Genres.Add(item);
        }

        public void Update(Genre item)
        {
            _db.SetModified(item);
        }

        public void Delete(int id)
        {
            var genre = _db.Genres.Find(id);
            if (genre != null)
            {
                _db.Genres.Remove(genre);
            }
        }

        public int GetCount(Expression<Func<Genre, bool>> predicate = null)
        {
            return predicate == null ? _db.Genres.Count() : _db.Genres.Count(predicate);
        }
    }
}
