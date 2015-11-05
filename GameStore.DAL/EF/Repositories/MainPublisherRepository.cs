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
    public class MainPublisherRepository : IPublisherRepository
    {
        private IContext _db;
        private IReferenceManager<Publisher> _publishersMangager;

        public MainPublisherRepository(IContext db, IReferenceManager<Publisher> publishersMangager)
        {
            _db = db;
            _publishersMangager = publishersMangager;
        }

        public Publisher Get(int id)
        {
            var publisher = _db.Publishers.AsNoTracking().FirstOrDefault(x => x.Id == id);

            _publishersMangager.Sync(publisher, DatabaseNames.GameStore);
            return publisher;
        }

        public IEnumerable<Publisher> Get()
        {
            var publishers = _db.Publishers.AsNoTracking().ToList();

            _publishersMangager.Sync(publishers, DatabaseNames.GameStore);
            return publishers;
        }

        public IEnumerable<Publisher> Get(Func<Publisher, bool> predicate)
        {
            var publishers = _db.Publishers.AsNoTracking().Where(predicate).ToList();

            _publishersMangager.Sync(publishers, DatabaseNames.GameStore);
            return publishers;
        }

        public Publisher GetSingle(Func<Publisher, bool> predicate)
        {
            var publisher = _db.Publishers.AsNoTracking().FirstOrDefault(predicate);

            _publishersMangager.Sync(publisher, DatabaseNames.GameStore);
            return publisher;
        }

        public void Add(Publisher item)
        {
            _db.Publishers.Add(item);
        }

        public void Update(Publisher item)
        {
            _db.SetModified(item);
        }

        public void Delete(int id)
        {
            var publisher = _db.Publishers.Find(id);
            if (publisher != null)
            {
                _db.Publishers.Remove(publisher);
            }
        }

        public int GetCount(Expression<Func<Publisher, bool>> predicate = null)
        {
            return predicate == null ? _db.Publishers.Count() : _db.Publishers.Count(predicate);
        }
    }
}
