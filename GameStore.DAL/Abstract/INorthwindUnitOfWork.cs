using System.Collections.Generic;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Abstract
{
    public interface INorthwindUnitOfWork
    {
        IOutRepository<Game> Games { get; }

        IOutRepository<Publisher> Publishers { get; }

        IOutRepository<Order> Orders { get; }

        IOutRepository<Genre> Genres { get; }

        IEnumerable<Shipper> GetShippers { get; } 
    }
}
