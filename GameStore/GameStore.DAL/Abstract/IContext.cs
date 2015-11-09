using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Abstract
{
    public interface IContext : IDisposable
    {
        IDbSet<Comment> Comments { get; }

        IDbSet<Game> Games { get; }

        IDbSet<Genre> Genres { get; }

        IDbSet<PlatformType> PlatformTypes { get; }

        IDbSet<Publisher> Publishers { get; }

        IDbSet<OrderDetails> OrderDetails { get; }

        IDbSet<User> Users { get; }

        IDbSet<Order> Orders { get; }

        /// <summary>
        /// Get's set of items if exists
        /// </summary>
        /// <typeparam name="T">Set type</typeparam>
        /// <returns></returns>
        IDbSet<T> Set<T>() where T : class;

        /// <summary>
        /// Sets EntryState bit into modified position
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        void SetModified<T>(T item) where T : class;

        Int32 SaveChanges();
    }
}
