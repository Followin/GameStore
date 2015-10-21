using System;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract
{
    public interface IGameStoreUnitOfWork : IUnitOfWork
    {
        IRepository<Genre, Int32> Genres { get; }

        IRepository<Game, Int32> Games { get; }

        IRepository<Comment, Int32> Comments { get; }

        IRepository<PlatformType, Int32> PlatformTypes { get; }

        IRepository<Publisher, Int32> Publishers { get; }

        IRepository<OrderDetails, Int32> OrderDetails { get; }

        IRepository<Order, Int32> Orders { get; } 
    }
}
