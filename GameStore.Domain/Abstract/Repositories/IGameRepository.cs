using System;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract.Repositories
{
    public interface IGameRepository : IRepository<Game, Int32>
    {

    }
}
