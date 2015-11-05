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
    public class MainPlatformTypeRepository : IPlatformTypeRepository
    {
        private IContext _db;

        public MainPlatformTypeRepository(IContext db)
        {
            _db = db;
        }

        public PlatformType Get(int id)
        {
            return _db.PlatformTypes.Find(id);
        }

        public IEnumerable<PlatformType> Get()
        {
            return _db.PlatformTypes.ToList();
        }

        public IEnumerable<PlatformType> Get(Func<PlatformType, bool> predicate)
        {
            return _db.PlatformTypes.Where(predicate);
        }

        public PlatformType GetSingle(Func<PlatformType, bool> predicate)
        {
            return _db.PlatformTypes.FirstOrDefault(predicate);
        }

        public void Add(PlatformType item)
        {
            _db.PlatformTypes.Add(item);
        }

        public void Update(PlatformType item)
        {
            _db.SetModified(item);
        }

        public void Delete(int id)
        {
            var platformType = _db.PlatformTypes.Find(id);
            if (platformType != null)
            {
                _db.PlatformTypes.Remove(platformType);
            }
        }

        public int GetCount(Expression<Func<PlatformType, bool>> predicate = null)
        {
            return predicate == null ? _db.PlatformTypes.Count() : _db.PlatformTypes.Count(predicate);
        }
    }
}
