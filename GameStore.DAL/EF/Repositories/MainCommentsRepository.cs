using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF.Repositories
{
    public class MainCommentsRepository : ICommentRepository
    {
        private IContext _db;

        public MainCommentsRepository(IContext db)
        {
            _db = db;
        }

        public Comment Get(int id)
        {
            return _db.Comments.Find(id);
        }

        public IEnumerable<Comment> Get()
        {
            return _db.Comments.ToList();
        }

        public IEnumerable<Comment> Get(Func<Comment, bool> predicate)
        {
            return _db.Comments.Where(predicate).ToList();
        }

        public Comment GetSingle(Func<Comment, bool> predicate)
        {
            return _db.Comments.FirstOrDefault(predicate);
        }

        public void Add(Comment item)
        {
            _db.Comments.Add(item);
        }

        public void Update(Comment item)
        {
            _db.SetModified(item);
        }

        public void Delete(int id)
        {
            var comment = _db.Comments.Find(id);
            if (comment != null)
            {
                _db.Comments.Remove(comment);
            }
        }

        public int GetCount(Expression<Func<Comment, bool>> predicate = null)
        {
            return predicate == null ? _db.Comments.Count() : _db.Comments.Count(predicate);
        }
    }
}
