using GameStore.DAL.Abstract.Repositories;

namespace GameStore.DAL.Abstract
{
    public interface IGameStoreUnitOfWork : IUnitOfWork
    {
        IGenreRepository Genres { get; }

        IGameRepository Games { get; }

        ICommentRepository Comments { get; }

        IPlatformTypeRepository PlatformTypes { get; }

        IPublisherRepository Publishers { get; }

        IOrderRepository Orders { get; }

        IUserRepository Users { get; }
    }
}
