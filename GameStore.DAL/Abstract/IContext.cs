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

        IDbSet<T> Set<T>() where T : class;

        void SetModified<T>(T item) where T : class;

        Int32 SaveChanges();
    }
}
