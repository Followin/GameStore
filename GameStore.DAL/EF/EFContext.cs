using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using GameStore.DAL.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF
{
    public class EFContext : DbContext, IContext
    {
        public EFContext(String connectionString)
            : base(connectionString)
        {
            Database.SetInitializer(new EFContextInitializer());
        }

        public IDbSet<Comment> Comments { get; set; }

        public IDbSet<Game> Games { get; set; }

        public IDbSet<Genre> Genres { get; set; }

        public IDbSet<PlatformType> PlatformTypes { get; set; }

        public IDbSet<Publisher> Publishers { get; set; }

        public IDbSet<OrderDetails> OrderDetails { get; set; }

        public IDbSet<User> Users { get; set; }

        public IDbSet<Order> Orders { get; set; } 

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
