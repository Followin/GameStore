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
            _context = context;

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
            var rpg = context.Genres.Add(new Genre { Name = "RPG" });
            context.Genres.Add(new Genre { Name = "Sports" });
            var action = context.Genres.Add(new Genre { Name = "Action" });
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

            var cdProject = context.Publishers.Add(new Publisher
            {
                Id = 2,
                CompanyName = "CD Project",
                Description = "Poland private game developing company",
                HomePage = "https://www.cdprojekt.com/"
            });

            var bethesda = context.Publishers.Add(new Publisher
            {
                CompanyName = "Bethesda",
                Description = "American video game publisher. A subsidiary of ZeniMax Media.",
                HomePage = "http://bethsoft.com/"
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
                IncomeDate = new DateTime(2013, 9, 7),
                PublicationDate = new DateTime(2013, 9, 7),
                Price = 100,
                UnitsInStock = 1000
            });

            context.Games.Add(new Game
            {
                Name = "Fallout 3",
                Description = "Action role-playing open world video game developed by Bethesda Game Studios, and is the third major installment in the Fallout series.",
                Genres = new[] {action},
                Key = "fallout-3",
                PlatformTypes = new[] {desktop},
                Publisher = bethesda,
                Discontinued = false,
                IncomeDate = new DateTime(2008, 8, 28),
                PublicationDate = new DateTime(2009, 1, 1),
                Price = 30,
                UnitsInStock = 1000
            });

            context.Games.Add(new Game
            {
                Name = "Fallout 4",
                Description =
                    "200 years after a nuclear war, Fallout 4 is set in a post-apocalyptic Boston, in which the player character emerges from an underground bunker known as a Vault. Gameplay will be similar to Fallout 3. Completing quests and acquiring experience will level up the character, allowing for new abilities.",
                Genres = new[] {action},
                Key = "fallout-4",
                PlatformTypes = new[] {desktop},
                Publisher = bethesda,
                Discontinued = false,
                IncomeDate = new DateTime(2015, 10, 27),
                PublicationDate = new DateTime(2015, 6, 3),
                Price = 30,
                UnitsInStock = 1000
            });

            context.Games.Add(new Game
            {
                Name = "The Witcher 3: Wild Hunt",
                Description =
                    "Played in a third-person perspective, players control protagonist Geralt of Rivia, a Witcher who sets out on a long journey through the large land of Northern Kingdoms. Players battle against the world's many dangers using swords and magic, while interacting with non-player characters and completing side quests and main missions all to progress through the story. Players mostly travel by foot, or mounted on Geralt's horse Roach.",
                Key = "witcher-3",
                Genres = new[] {rpg},
                PlatformTypes = new[] {desktop},
                Publisher = cdProject,
                Discontinued = false,
                UnitsInStock = 1000,
                IncomeDate = new DateTime(2015, 5, 20),
                PublicationDate = new DateTime(2015, 5, 19),
                Price = 100
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
