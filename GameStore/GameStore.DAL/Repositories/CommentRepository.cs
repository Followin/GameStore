using System;
using GameStore.DAL.Abstract;
using GameStore.DAL.Abstract.Repositories;
using GameStore.DAL.EF;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class CommentRepository : GenericRepository<Comment, Int32>, ICommentRepository
    {
        public CommentRepository(IEFContext context) : base(context)
        {
        }

    }
}
