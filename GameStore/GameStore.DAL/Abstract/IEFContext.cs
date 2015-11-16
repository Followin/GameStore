using System;
using System.Data.Entity;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Abstract
{
    public interface IEFContext : IDisposable
    {
        IDbSet<Comment> Comments { get; set; }

        IDbSet<Game> Games { get; set; }

        IDbSet<Genre> Genres { get; set; }

        IDbSet<PlatformType> PlatformTypes { get; set; }

        IDbSet<Publisher> Publishers { get; set; }

        IDbSet<OrderDetails> OrderDetails { get; set; }

        IDbSet<User> Users { get; set; }

        IDbSet<UserClaim> UserClaims { get; set; }

        IDbSet<Order> Orders { get; set; }

        IDbSet<GameGenre> GamesGenres { get; set; }

        IDbSet<T> Set<T>() where T : class;

        void SetModified<T>(T item) where T : class;


        int SaveChanges();
    }
}