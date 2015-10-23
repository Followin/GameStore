using System;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract
{
    public interface IGameStoreUnitOfWork : IUnitOfWork
    {
        IGenreRepository Genres { get; }

        IGameRepository Games { get; }

        ICommentRepository Comments { get; }

        IPlatformTypeRepository PlatformTypes { get; }

        IPublisherRepository Publishers { get; }

        IOrderDetailsRepository OrderDetails { get; }

        IOrderRepository Orders { get; } 
    }
}
