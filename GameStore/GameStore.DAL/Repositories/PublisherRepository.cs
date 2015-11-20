using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Abstract;
using GameStore.DAL.Abstract.Repositories;
using GameStore.DAL.EF;
using GameStore.DAL.Static;
using GameStore.Domain.Entities;
using GameStore.Static;

namespace GameStore.DAL.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private IEFContext _db;
        private INorthwindUnitOfWork _northwind;

        public PublisherRepository(IEFContext db, INorthwindUnitOfWork north)
        {
            _db = db;
            _northwind = north;
        }

        public Publisher Get(int id)
        {
            var database = KeyEncoder.GetBase(id);
            Publisher publisher = null;

            switch (database)
            {
                case DatabaseTypes.GameStore:
                    publisher =  _db.Publishers.Find(id);
                    break;
                case DatabaseTypes.Northwind:
                    publisher =  _northwind.Publishers.Get(KeyEncoder.GetId(id));
                    break;
            }

            return publisher;
        }

        public IEnumerable<Publisher> Get()
        {
            var publishers = _db.Publishers.ToList();
            var exludeIds = publishers.Where(x => KeyEncoder.GetBase(x.Id) == DatabaseTypes.Northwind).Select(x => KeyEncoder.GetId(x.Id));
            publishers.AddRange(_northwind.Publishers.GetExluding(exludeIds));
            return publishers;
        }

        public IEnumerable<Publisher> Get(Expression<Func<Publisher, bool>> predicate)
        {
            return Get().Where(predicate.Compile());
        }

        public Publisher GetFirst(Expression<Func<Publisher, bool>> predicate)
        {
            return Get().FirstOrDefault(predicate.Compile());
        }

        public int GetCount(Expression<Func<Publisher, bool>> predicate = null)
        {
            return predicate == null
                ? Get().Count()
                : Get().Count(predicate.Compile());
        }

        public void Add(Publisher item)
        {
            var lastId = _db.Publishers.Select(x => x.Id).ToList().Where(x => KeyEncoder.GetBase(x) == DatabaseTypes.GameStore).Max(x => x);
            lastId = KeyEncoder.GetNext(lastId);
            item.Id = lastId;
            _db.Publishers.Add(item);
        }

        public void Delete(int id)
        {
            var database = KeyEncoder.GetBase(id);
            switch (database)
            {
                case DatabaseTypes.GameStore:
                    var publisher = _db.Publishers.Find(id);
                    publisher.EntryState = EntryState.Deleted;
                    _db.SetModified(publisher);
                    break;
                case DatabaseTypes.Northwind:
                    var nPublisher = _northwind.Publishers.Get(KeyEncoder.GetId(id));
                    nPublisher.EntryState = EntryState.Deleted;
                    _db.Publishers.Add(nPublisher);
                    break;
            }
        }

        public void Update(Publisher item)
        {
            var database = KeyEncoder.GetBase(item.Id);
            switch (database)
            {
                case DatabaseTypes.GameStore:
                    _db.SetModified(item);
                    break;
                case DatabaseTypes.Northwind:
                    _db.Publishers.Add(item);
                    break;
            }
        }
    }
}
