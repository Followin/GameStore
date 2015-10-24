using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class CommentRepository : GenericRepository<Comment, Int32>, ICommentRepository
    {
        public CommentRepository(IContext context) : base(context)
        {
        }

    }
}
