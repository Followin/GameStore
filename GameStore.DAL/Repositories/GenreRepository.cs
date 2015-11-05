using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Abstract;
using GameStore.DAL.EF;
using GameStore.DAL.Static;
using GameStore.Domain.Abstract;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private EFContext _db;
        private INorthwindUnitOfWork _northwind;

        public GenreRepository(EFContext db, INorthwindUnitOfWork northwind)
        {
            _db = db;
            _northwind = northwind;
        }


        public Genre Get(int id)
        {
            var database = KeyEncoder.GetBase(id);
            switch (database)
            {
                case DatabaseTypes.GameStore:
                    return _db.Genres.Find(id);
                case DatabaseTypes.Northwind:
                    return _northwind.Genres.Get(KeyEncoder.GetId(id));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IEnumerable<Genre> Get()
        {
            var genres = _db.Genres.ToList();
            var exludeIds = genres.Where(x => KeyEncoder.GetBase(x.Id) == DatabaseTypes.Northwind).Select(x => KeyEncoder.GetId(x.Id));
            genres.AddRange(_northwind.Genres.GetExluding(exludeIds));
            return genres;
        }

        public IEnumerable<Genre> Get(Expression<Func<Genre, bool>> predicate)
        {
            return Get().Where(predicate.Compile());
        }

        public Genre GetSingle(Expression<Func<Genre, bool>> predicate)
        {
            return Get().FirstOrDefault(predicate.Compile());
        }

        public int GetCount(Expression<Func<Genre, bool>> predicate = null)
        {
            return predicate == null
                ? Get().Count()
                : Get().Count(predicate.Compile());
        }

        public void Add(Genre item)
        {
            var lastId = _db.Genres.Max(x => x.Id);
            lastId += KeyEncoder.Coefficient;
            item.Id = lastId;
            _db.Genres.Add(item);
        }

        public void Delete(int id)
        {
            var database = KeyEncoder.GetBase(id);
            switch (database)
            {
                case DatabaseTypes.GameStore:
                    var genre = _db.Genres.Find(id);
                    genre.EntryState = EntryState.Deleted;
                    _db.Entry(genre).State = EntityState.Modified;
                    break;
                case DatabaseTypes.Northwind:
                    var nGenre = _northwind.Genres.Get(KeyEncoder.GetId(id));
                    nGenre.EntryState = EntryState.Deleted;
                    _db.Genres.Add(nGenre);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Update(Genre item)
        {
            var database = KeyEncoder.GetBase(item.Id);
            switch (database)
            {
                case DatabaseTypes.GameStore:
                    _db.Entry(item).State = EntityState.Modified;
                    break;
                case DatabaseTypes.Northwind:
                    _db.Genres.Add(item);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}