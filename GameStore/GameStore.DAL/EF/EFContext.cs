using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF
{
    public class EFContext : DbContext
    {
        public EFContext(String connectionString)
            : base(connectionString)
        {
           //Database.SetInitializer(new EFContextInitializer());
        }

        public IDbSet<Comment> Comments { get; set; }

        public IDbSet<Game> Games { get; set; }

        public IDbSet<Genre> Genres { get; set; }

        public IDbSet<PlatformType> PlatformTypes { get; set; }

        public IDbSet<Publisher> Publishers { get; set; }

        public IDbSet<OrderDetails> OrderDetails { get; set; }

        public IDbSet<User> Users { get; set; }

        public IDbSet<UserClaim> UserClaims { get; set; } 

        public IDbSet<Order> Orders { get; set; }

        public IDbSet<GameGenre> GamesGenres { get; set; }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public void SetModified<T>(T item) where T : class
        {
            Entry(item).State = EntityState.Modified;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GameConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());
            modelBuilder.Configurations.Add(new GenreConfiguration());
            modelBuilder.Configurations.Add(new PlatformTypeConfiguration());
            modelBuilder.Configurations.Add(new PublisherConfiguration());
            modelBuilder.Configurations.Add(new OrderDetailsConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new GameGenreConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }

    public class EFContextFactory : IDbContextFactory<EFContext>
    {
        public EFContext Create()
        {
            return new EFContext("DefaultConnection");
        }
    }
}
