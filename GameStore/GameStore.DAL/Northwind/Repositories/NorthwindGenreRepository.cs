using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.DAL.Abstract;
using GameStore.DAL.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Northwind.Repositories
{
    public class NorthwindGenreRepository : IOutRepository<Genre>
    {
        private NorthwindContext _db;

        public NorthwindGenreRepository(NorthwindContext db)
        {
            _db = db;
        }

        private IEnumerable<Category> Categories
        {
            get { return _db.Categories; }
        }

        public Genre Get(int id)
        {
            return Mapper.Map<Category, Genre>(Categories.FirstOrDefault(x => x.CategoryID == id));
        }

        public IEnumerable<Genre> Get()
        {
            return Mapper.Map<IEnumerable<Category>, IEnumerable<Genre>>(Categories.ToList());
        }

        public IEnumerable<Genre> GetExluding(IEnumerable<int> exludingIds)
        {
            return Mapper.Map<IEnumerable<Category>, IEnumerable<Genre>>(Categories.Where(x => !exludingIds.Contains(x.CategoryID)).ToList());
        }

        public IEnumerable<Genre> GetIncluding(IEnumerable<int> includingIds)
        {
            return Mapper.Map<IEnumerable<Category>, IEnumerable<Genre>>(Categories.Where(x => includingIds.Contains(x.CategoryID)).ToList());
        }
    }
}
