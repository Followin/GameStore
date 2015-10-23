using System;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract.Repositories
{
    public interface ICommentRepository : IRepository<Comment, Int32>
    {
         
    }
}