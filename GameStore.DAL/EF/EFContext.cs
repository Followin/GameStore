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
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<Game> Games { get; set; }
        public IDbSet<Genre> Genres { get; set; }
        public IDbSet<PlatformType> PlatformTypes { get; set; }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public void SetModified<T>(T item) where T : class
        {
            Entry(item).State = EntityState.Modified;
        }

        public EFContext(String connectionString) : base(connectionString)
        {
            Database.SetInitializer(new EFContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GameConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());
            modelBuilder.Configurations.Add(new GenreConfiguration());
            modelBuilder.Configurations.Add(new PlatformTypeConfiguration());
        }
    }

    public class EFContextInitializer : DropCreateDatabaseAlways<EFContext>
    {
        private EFContext context;

        protected override void Seed(EFContext context)
        {
            this.context = context;

            //unique indexes
            CreateIndex("Key", typeof(Game));
            CreateIndex("Name", typeof(Genre));
            CreateIndex("Name", typeof(PlatformType));

            //Data
            context.Genres.Add(new Genre
            {
                Name = "Strategy",
                ChildGenres = new[]
                {
                    new Genre {Name = "RTS"},
                    new Genre {Name = "TBS"}
                }
            });
            context.Genres.Add(new Genre
            {
                Name = "Races",
                ChildGenres = new[]
                {
                    new Genre {Name="Rally"},
                    new Genre {Name="Arcade"},
                    new Genre {Name="Formula"},
                    new Genre {Name="Off-road"}
                }
            });
            context.Genres.Add(new Genre { Name = "RPG" });
            context.Genres.Add(new Genre { Name = "Sports" });
            context.Genres.Add(new Genre { Name = "Action" });
            context.Genres.Add(new Genre { Name = "Adventure" });
            context.Genres.Add(new Genre { Name = "Puzzle&Skill" });
            var moba = context.Genres.Add(new Genre {Name = "MOBA"});


            context.PlatformTypes.Add(new PlatformType { Name = "Mobile" });
            context.PlatformTypes.Add(new PlatformType { Name = "Browser" });
            var desktop = context.PlatformTypes.Add(new PlatformType { Name = "Desktop" });
            context.PlatformTypes.Add(new PlatformType { Name = "Console" });

            var dota = context.Games.Add(new Game
            {
                Name = "Dota 2",
                Description = "Just try it",
                Genres = new[] {moba},
                Key = "dota-2",
                PlatformTypes = new[] {desktop}
            });

            context.Comments.Add(new Comment
            {
                Game = dota,
                Name = "FirstAuthor",
                Body = "Trully amazing one",
                ChildComments = new[]
                {
                    new Comment() {Name = "SecondAuthor", Game = dota, Body = "Can't disagree"}
                }
            });


            context.SaveChanges();
        }

        private void CreateIndex(string field, Type table)
        {
            var command = String.Format("CREATE UNIQUE INDEX IX_{0} ON [{1}s]([{0}])", field, table.Name);
            context.Database.ExecuteSqlCommand(command);
        }
    }

    public class EFContextFactory : IDbContextFactory<EFContext>
    {
        public EFContext Create()
        {
            return new EFContext("DefaultConnection");
        }
    }

    public class GameConfiguration : EntityTypeConfiguration<Game>
    {
        public GameConfiguration()
        {
            Property(x => x.Key).IsRequired()
                                .HasMaxLength(50);
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(50);
            Property(x => x.Description).IsRequired();
        }
    }

    public class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(30);
            Property(x => x.Body).IsRequired();
        }
    }

    public class GenreConfiguration : EntityTypeConfiguration<Genre>
    {
        public GenreConfiguration()
        {
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(50);
        }
    }

    public class PlatformTypeConfiguration : EntityTypeConfiguration<PlatformType>
    {
        public PlatformTypeConfiguration()
        {
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(20);
        }
    }
}
