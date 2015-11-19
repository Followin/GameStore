using System;
using System.Data.Entity;
using System.Security.Claims;
using GameStore.DAL.Static;
using GameStore.Domain.Entities;
using GameStore.Static;

namespace GameStore.DAL.EF
{
    public class EFContextInitializer : DropCreateDatabaseAlways<EFContext>
    {
        private EFContext _context;

        private Int32 GetNextId(ref Int32 id)
        {
            id = KeyEncoder.GetNext(id);
            return id;
        }

        protected override void Seed(EFContext context)
        {
            _context = context;

            // unique indexes
            CreateIndex("Key", typeof(Game));
            CreateIndex("NameEn", typeof(Genre));
            CreateIndex("NameRu", typeof(Genre));
            CreateIndex("Name", typeof(PlatformType));

            var genreId = (Int32)DatabaseTypes.GameStore;

            // Data
            context.Genres.Add(new Genre
            {
                Id = GetNextId(ref genreId),
                NameEn = "Strategy",
                NameRu = "Стратегия",
                ChildGenres = new[]
                {
                    new Genre { Id = GetNextId(ref genreId), NameRu = "RTS", NameEn = "RTS" },
                    new Genre { Id = GetNextId(ref genreId), NameRu = "TBS", NameEn = "TBS" }
                }
            });
            context.Genres.Add(new Genre
            {
                Id = GetNextId(ref genreId),
                NameEn = "Races",
                NameRu = "Гонки",
                ChildGenres = new[]
                {
                    new Genre { Id = GetNextId(ref genreId), NameEn = "Rally", NameRu = "Ралли" },
                    new Genre { Id = GetNextId(ref genreId), NameEn = "Arcade", NameRu = "Аркада" },
                    new Genre { Id = GetNextId(ref genreId), NameEn = "Formula", NameRu = "Формула" },
                    new Genre { Id = GetNextId(ref genreId), NameEn = "Off-road", NameRu = "Гонки по бездороъю" }
                }
            });
            var rpg = context.Genres.Add(new Genre { Id = GetNextId(ref genreId), NameEn = "RPG", NameRu = "RPG" });
            context.Genres.Add(new Genre { Id = GetNextId(ref genreId), NameEn = "Sports", NameRu = "Спорт" });
            var action = context.Genres.Add(new Genre { Id = GetNextId(ref genreId), NameEn = "Action", NameRu = "Action" });
            context.Genres.Add(new Genre { Id = GetNextId(ref genreId), NameEn = "Adventure", NameRu = "Приключения" });
            context.Genres.Add(new Genre { Id = GetNextId(ref genreId), NameEn = "Puzzle&Skill", NameRu = "Головоломка" });
            var moba = context.Genres.Add(new Genre { Id = GetNextId(ref genreId), NameEn = "MOBA", NameRu = "MOBA" });

            context.PlatformTypes.Add(new PlatformType { Name = "Mobile" });
            context.PlatformTypes.Add(new PlatformType { Name = "Browser" });
            var desktop = context.PlatformTypes.Add(new PlatformType { Name = "Desktop" });
            context.PlatformTypes.Add(new PlatformType { Name = "Console" });

            var publisherId = (Int32) DatabaseTypes.GameStore;

            var valve = context.Publishers.Add(new Publisher
            {
                Id = GetNextId(ref publisherId),
                CompanyName = "Valve",
                Description = "Greed Gaben's company",
                HomePage = "http://www.valvesoftware.com/",
            });

            var cdProject = context.Publishers.Add(new Publisher
            {
                Id = GetNextId(ref publisherId),
                CompanyName = "CD Project",
                Description = "Poland private game developing company",
                HomePage = "https://www.cdprojekt.com/"
            });

            var bethesda = context.Publishers.Add(new Publisher
            {
                Id = GetNextId(ref publisherId),
                CompanyName = "Bethesda",
                Description = "American video game publisher. A subsidiary of ZeniMax Media.",
                HomePage = "http://bethsoft.com/"
            });

            var gameId = (Int32) DatabaseTypes.GameStore;

            var dota = context.Games.Add(new Game
            {
                Id = GetNextId(ref gameId),
                Name = "Dota 2",
                DescriptionEn = "Just try it",
                DescriptionRu = "компьютерная многопользовательская командная игра жанра Multiplayer online battle arena, реализация известной карты DotA для игры Warcraft III в отдельном клиенте.",
                Key = "dota-2",
                PlatformTypes = new[] { desktop },
                PublisherId = valve.Id,
                Discontinued = false,
                IncomeDate = new DateTime(2013, 9, 7),
                PublicationDate = new DateTime(2013, 9, 7),
                Price = 100,
                UnitsInStock = 1000
            });

            var fallout3 = context.Games.Add(new Game
            {
                Id = GetNextId(ref gameId),
                Name = "Fallout 3",
                DescriptionRu = "Сюжет Fallout 3 продолжает развитие событий серии игр Fallout, действие которых происходит в мире, медленно возрождающемся после ядерной войны 2077 года. Время действия игры — 2277 год, её события происходят через 36 лет после событий игры Fallout 2. Как и в предыдущих играх серии, в Fallout 3 фигурирует сеть подземных убежищ, построенная корпорацией Vault-Tec, как заявлялось, для спасения некоторой части населения Америки от последствий войны, хотя и вне убежищ присутствуют выжившие люди, находящиеся в гораздо меньшей безопасности. Главный герой игры — Одинокий странник, с малых лет живший в убежище № 101. Хотя по его представлениям убежище с момента ядерной войны ни разу не открывалось, его отец однажды уходит наружу. Одинокий странник отправляется на его поиски, чтобы узнать причины ухода.", 
                DescriptionEn = "Action role-playing open world video game developed by Bethesda Game Studios, and is the third major installment in the Fallout series.",
                Key = "fallout-3",
                PlatformTypes = new[] {desktop},
                PublisherId = bethesda.Id,
                Discontinued = false,
                IncomeDate = new DateTime(2008, 8, 28),
                PublicationDate = new DateTime(2009, 1, 1),
                Price = 30,
                UnitsInStock = 1000,
                EntryState = EntryState.Deleted
            });

            var fallout4 = context.Games.Add(new Game
            {
                Id = GetNextId(ref gameId),
                Name = "Fallout 4",
                DescriptionRu = "Игра является пятой частью серии, и будет выпущена 10 ноября 2015 года на PC, PS4 и Xbox One. На территории России и СНГ локализуется компанией СофтКлаб.", 
                DescriptionEn = 
                    "200 years after a nuclear war, Fallout 4 is set in a post-apocalyptic Boston, in which the player character emerges from an underground bunker known as a Vault. Gameplay will be similar to Fallout 3. Completing quests and acquiring experience will level up the character, allowing for new abilities.",
                Key = "fallout-4",
                PlatformTypes = new[] {desktop},
                PublisherId = bethesda.Id,
                Discontinued = false,
                IncomeDate = new DateTime(2015, 10, 27),
                PublicationDate = new DateTime(2015, 6, 3),
                Price = 30,
                UnitsInStock = 1000
                
            });

            var witcher3 = context.Games.Add(new Game
            {
                Id = GetNextId(ref gameId),
                Name = "The Witcher 3: Wild Hunt",
                DescriptionRu = "Действие игры происходит в вымышленном фэнтезийном мире, напоминающем средневековую Европу. Главный герой Геральт из Ривии, «ведьмак» — профессиональный охотник на чудовищ — отправляется в путешествие в поисках девушки по имени Цири, обладающей сверхъестественными способностями. В отличие от предыдущих игр серии, «Ведьмак 3: Дикая Охота» — игра с открытым миром: игрок может свободно путешествовать по обширным территориям, самостоятельно находя новые места и задания.",
                DescriptionEn =
                    "Played in a third-person perspective, players control protagonist Geralt of Rivia, a Witcher who sets out on a long journey through the large land of Northern Kingdoms. Players battle against the world's many dangers using swords and magic, while interacting with non-player characters and completing side quests and main missions all to progress through the story. Players mostly travel by foot, or mounted on Geralt's horse Roach.",
                Key = "witcher-3",
                PlatformTypes = new[] {desktop},
                PublisherId = cdProject.Id,
                Discontinued = false,
                UnitsInStock = 1000,
                IncomeDate = new DateTime(2015, 5, 20),
                PublicationDate = new DateTime(2015, 5, 19),
                Price = 100
            });

            context.GamesGenres.Add(new GameGenre {GameId = dota.Id, GenreId = moba.Id});
            context.GamesGenres.Add(new GameGenre { GameId = witcher3.Id, GenreId = rpg.Id });
            context.GamesGenres.Add(new GameGenre { GameId = fallout3.Id, GenreId = action.Id });
            context.GamesGenres.Add(new GameGenre { GameId = fallout4.Id, GenreId = action.Id });

            context.Comments.Add(new Comment
            {
                GameId = dota.Id,
                Name = "FirstAuthor",
                Body = "Trully amazing one",
                ChildComments = new[]
                {
                    new Comment() { Name = "SecondAuthor", GameId = dota.Id, Body = "Can't disagree" }
                }
            });

            context.Users.Add(new User
            {
                Name = "Admin",
                PasswordHash = "ABOB+2NtMPelLA77KE4KTt/EztyZqH1aZ9+eMIwYkBhWUcViMyhYj2Zf9arebVTTcQ==",
                SecurityStamp = Guid.NewGuid().ToString(),
                Claims = new[]
                {
                    new UserClaim
                    {
                        Type = ClaimTypes.Role,
                        Issuer = "GameStore",
                        Value = Roles.Admin
                    }
                }
            });

            context.Users.Add(new User
            {
                Name = "Moderator",
                PasswordHash = "ABOB+2NtMPelLA77KE4KTt/EztyZqH1aZ9+eMIwYkBhWUcViMyhYj2Zf9arebVTTcQ==",
                SecurityStamp = Guid.NewGuid().ToString(),
                Claims = new[]
                {
                    new UserClaim
                    {
                        Type = ClaimTypes.Role,
                        Issuer = "GameStore",
                        Value = Roles.Moderator
                    }
                }
            });

            context.Users.Add(new User
            {
                Name = "Manager",
                PasswordHash = "ABOB+2NtMPelLA77KE4KTt/EztyZqH1aZ9+eMIwYkBhWUcViMyhYj2Zf9arebVTTcQ==",
                SecurityStamp = Guid.NewGuid().ToString(),
                Claims = new[]
                {
                    new UserClaim
                    {
                        Type = ClaimTypes.Role,
                        Issuer = "GameStore",
                        Value = Roles.Manager
                    }
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
