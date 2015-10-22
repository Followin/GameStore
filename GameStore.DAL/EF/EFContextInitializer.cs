using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF
{
    public class EFContextInitializer : DropCreateDatabaseAlways<EFContext>
    {
        private EFContext _context;

        protected override void Seed(EFContext context)
        {
            this._context = context;

            // unique indexes
            CreateIndex("Key", typeof(Game));
            CreateIndex("Name", typeof(Genre));
            CreateIndex("Name", typeof(PlatformType));

            // Data
            context.Genres.Add(new Genre
            {
                Name = "Strategy",
                ChildGenres = new[]
                {
                    new Genre { Name = "RTS" },
                    new Genre { Name = "TBS" }
                }
            });
            context.Genres.Add(new Genre
            {
                Name = "Races",
                ChildGenres = new[]
                {
                    new Genre { Name = "Rally" },
                    new Genre { Name = "Arcade" },
                    new Genre { Name = "Formula" },
                    new Genre { Name = "Off-road" }
                }
            });
            context.Genres.Add(new Genre { Name = "RPG" });
            context.Genres.Add(new Genre { Name = "Sports" });
            context.Genres.Add(new Genre { Name = "Action" });
            context.Genres.Add(new Genre { Name = "Adventure" });
            context.Genres.Add(new Genre { Name = "Puzzle&Skill" });
            var moba = context.Genres.Add(new Genre { Name = "MOBA" });

            context.PlatformTypes.Add(new PlatformType { Name = "Mobile" });
            context.PlatformTypes.Add(new PlatformType { Name = "Browser" });
            var desktop = context.PlatformTypes.Add(new PlatformType { Name = "Desktop" });
            context.PlatformTypes.Add(new PlatformType { Name = "Console" });

            var valve = context.Publishers.Add(new Publisher
            {
                CompanyName = "Valve",
                Description = "Greed Gaben's company",
                HomePage = "http://www.valvesoftware.com/",
            });

            var dota = context.Games.Add(new Game
            {
                Name = "Dota 2",
                Description = "Just try it",
                Genres = new[] { moba },
                Key = "dota-2",
                PlatformTypes = new[] { desktop },
                Publisher = valve,
                Discontinued = false,
                Price = 100,
                UnitsInStock = 1000
            });

            context.Comments.Add(new Comment
            {
                Game = dota,
                Name = "FirstAuthor",
                Body = "Trully amazing one",
                ChildComments = new[]
                {
                    new Comment() { Name = "SecondAuthor", Game = dota, Body = "Can't disagree" }
                }
            });

            context.SaveChanges();
        }

        private void CreateIndex(string field, Type table)
        {
            var command = String.Format("CREATE UNIQUE INDEX IX_{0} ON [{1}s]([{0}])", field, table.Name);
            _context.Database.ExecuteSqlCommand(command);
        }
    }
}
