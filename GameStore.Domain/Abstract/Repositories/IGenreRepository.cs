using System;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract.Repositories
{
    public interface IGenreRepository : IRepository<Genre, Int32>
    {
         
    }
}