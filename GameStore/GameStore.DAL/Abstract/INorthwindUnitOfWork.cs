using System.Collections.Generic;
using GameStore.DAL.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Abstract
{
    public interface INorthwindUnitOfWork
    {
        /// <summary>
        /// Games repository
        /// </summary>
        IOutRepository<Game> Games { get; }

        /// <summary>
        /// Publishers repository
        /// </summary>
        IOutRepository<Publisher> Publishers { get; }

        /// <summary>
        /// Orders repository
        /// </summary>
        IOutRepository<Order> Orders { get; }

        /// <summary>
        /// Genres repository
        /// </summary>
        IOutRepository<Genre> Genres { get; }

        /// <summary>
        /// Shippers repository
        /// </summary>
        IOutRepository<Shipper> Shippers { get; } 
    }
}
