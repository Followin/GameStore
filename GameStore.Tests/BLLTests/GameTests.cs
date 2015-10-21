using System;
using System.Linq;
using System.Linq.Expressions;
using GameStore.BLL.CommandHandlers;
using GameStore.BLL.Commands;
using GameStore.BLL.Queries;
using GameStore.BLL.QueryHandlers;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;

namespace GameStore.Tests.BLLTests
{
    [TestClass]
    public class GameHandlerTests
    {
        private GameCommandHandler commandHandler;
        private GameQueryHandler queryHandler;
        private Mock<IRepository<Game, Int32>> gameRepositoryMock;
        private Mock<IRepository<Genre, Int32>> genreRepositoryMock;
        private Mock<IRepository<PlatformType, Int32>> platformTypeRepositoryMock;
        private Mock<IGameStoreUnitOfWork> unitOfWorkMock;
        private CreateGameCommand newGameRightCommand;
        private EditGameCommand editGameRightCommand;
        private Game dota;
        private Game witcher;
        private Game[] games;

        [ClassInitialize]
        public static void Initializer(TestContext testContext)
        {
            AutoMapperConfiguration.Configure();
        }

        [TestInitialize]
        public void TestInializer()
        {
            var rts = new Genre
            {
                Id = 1,
                Name = "RTS",
                ChildGenres = new Genre[0]
            };

            var strategy = new Genre
            {
                Id = 2,
                Name = "Strategy",
                ChildGenres = new[] { rts }
            };

            rts.ParentGenre = strategy;
            rts.ParentGenreId = 2;
            var genres = new[] { rts, strategy };
            genreRepositoryMock = new Mock<IRepository<Genre, int>>();
            genreRepositoryMock.Setup(x => x.Get()).Returns(genres);
            genreRepositoryMock.Setup(x => x.Get(It.IsAny<Int32>())).Returns(
                (Int32 i) => genres.FirstOrDefault(g => g.Id == i));
            genreRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<Genre, Boolean>>>())).Returns(
                (Expression<Func<Genre, Boolean>> predicate) => genres.FirstOrDefault(predicate.Compile()));
            
            var desktop = new PlatformType
            {
                Id = 1,
                Name = "Desktop"
            };
            var web = new PlatformType
            {
                Id = 2,
                Name = "Web"
            };
            var platformTypes = new[] { desktop, web };
            platformTypeRepositoryMock = new Mock<IRepository<PlatformType, int>>();
            platformTypeRepositoryMock.Setup(x => x.Get()).Returns(platformTypes);
            platformTypeRepositoryMock.Setup(x => x.Get(It.IsAny<Int32>())).Returns(
                (Int32 i) => platformTypes.FirstOrDefault(g => g.Id == i));
            platformTypeRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<PlatformType, Boolean>>>())).Returns(
                (Expression<Func<PlatformType, Boolean>> predicate) => platformTypes.FirstOrDefault(predicate.Compile()));
            platformTypeRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<PlatformType, Boolean>>>())).Returns(
                (Expression<Func<PlatformType, Boolean>> predicate) => platformTypes.Where(predicate.Compile()));

            newGameRightCommand = new CreateGameCommand
            {
                Name = "GTA 5",
                Description = "5 part",
                Key = "gta-5",
                GenreIds = new[] { 1 },
                PlatformTypeIds = new[] { 1 }
            };

            editGameRightCommand = new EditGameCommand
            {
                Id = 1,
                Name = "New name",
                Description = "New description",
                Key = "new-key",
                GenreIds = new[] { 2 },
                PlatformTypeIds = new[] { 2 }
            };

            dota = new Game
            {
                Id = 1,
                Name = "Dota 2",
                Description = "Just try it",
                Key = "dota-2",
                Genres = new[] { rts },
                PlatformTypes = new[] { desktop, web }
            };

            witcher = new Game
            {
                Id = 2,
                Name = "Witcher 3",
                Description = "3d part of trilogy",
                Key = "witcher-3",
                Genres = new[] { strategy },
                PlatformTypes = new[] { desktop }
            };
            games = new[] { dota, witcher };
            gameRepositoryMock = new Mock<IRepository<Game, int>>();
            gameRepositoryMock.Setup(x => x.Get()).Returns(games);
            gameRepositoryMock.Setup(x => x.Get(It.Is<Int32>(i => i == 1))).Returns(dota);
            gameRepositoryMock.Setup(x => x.Get(It.Is<Int32>(i => i == 2))).Returns(witcher);
            gameRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Game, Boolean>>>())).Returns(
                (Expression<Func<Game, Boolean>> predicate) => games.Where(predicate.Compile()));
            gameRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<Game, Boolean>>>())).Returns(
                (Expression<Func<Game, Boolean>> predicate) => games.FirstOrDefault(predicate.Compile()));

            unitOfWorkMock = new Mock<IGameStoreUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Games).Returns(gameRepositoryMock.Object);
            unitOfWorkMock.Setup(x => x.Genres).Returns(genreRepositoryMock.Object);
            unitOfWorkMock.Setup(x => x.PlatformTypes).Returns(platformTypeRepositoryMock.Object);

            var logger = new Mock<ILogger>();
            commandHandler = new GameCommandHandler(unitOfWorkMock.Object, logger.Object);
            queryHandler = new GameQueryHandler(unitOfWorkMock.Object, logger.Object);
        }

        #region Create_Tests
        [TestMethod]
        public void Create_Game_Name_Argument_Is_Null()
        {
            // Arrange
            newGameRightCommand.Name = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() => 
                commandHandler.Execute(newGameRightCommand));

            // Assert
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_Name_Argument_Is_Empty()
        {
            // Arrange
            newGameRightCommand.Name = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(newGameRightCommand));

            // Assert
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_Key_Argument_Is_Null()
        {
            // Arrange
            newGameRightCommand.Key = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() => 
                commandHandler.Execute(newGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_Key_Argument_Is_Empty()
        {
            // Arrange
            newGameRightCommand.Key = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(newGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_Description_Argument_Is_Null()
        {
            // Arrange
            newGameRightCommand.Description = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() => 
                commandHandler.Execute(newGameRightCommand));

            // Assert
            Assert.AreEqual("Description", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_Description_Argument_Is_Empty()
        {
            // Arrange
            newGameRightCommand.Description = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(newGameRightCommand));

            // Assert
            Assert.AreEqual("Description", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_GenresIds_Argument_Is_Null()
        {
            // Arrange
            newGameRightCommand.GenreIds = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                commandHandler.Execute(newGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_GenreIds_Argument_Is_Empty()
        {
            // Arrange
            newGameRightCommand.GenreIds = Enumerable.Empty<Int32>().ToArray();

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(newGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_PlatformTypeIds_Argument_Is_Null()
        {
            // Arrange
            newGameRightCommand.PlatformTypeIds = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                commandHandler.Execute(newGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_PlatformTypeIds_Argument_Is_Empty()
        {
            // Arrange
            newGameRightCommand.PlatformTypeIds = Enumerable.Empty<Int32>().ToArray();

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(newGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_With_Existing_Key()
        {
            // Arrange
            newGameRightCommand.Key = "dota-2";
            gameRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<Game, Boolean>>>()))
                .Returns((Expression<Func<Game, Boolean>> predicate) => predicate.Compile()(dota) ? dota : null);

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(newGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_With_Non_Exising_Genre()
        {
            // Arrange
            newGameRightCommand.GenreIds = new[] { 1, 5 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(newGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_With_Non_Exising_PlatformType()
        {
            // Arrange
            newGameRightCommand.PlatformTypeIds = new[] { 1, 5 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(newGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_With_Right_Data()
        {
            // Arrange
            // Act
            commandHandler.Execute(newGameRightCommand);

            // Assert
            gameRepositoryMock.Verify(x => x.Add(It.IsAny<Game>()), Times.Once);
            unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }

        #endregion

        #region Edit_Tests

        [TestMethod]
        public void Edit_Game_Id_Argument_Is_Zero()
        {
            // Arrange
            editGameRightCommand.Id = 0;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            gameRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Never());
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Id_Argument_Lower_Than_Zero()
        {
            // Arrange
            editGameRightCommand.Id = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => 
                commandHandler.Execute(editGameRightCommand));

            // Assert
            gameRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Never());
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Id_Argument_Doesnt_Match_Exising_Game()
        {
            // Arrange
            editGameRightCommand.Id = 5;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Name_Argument_Is_Null()
        {
            // Arrange
            editGameRightCommand.Name = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Name_Argument_Is_Empty()
        {
            // Arrange
            editGameRightCommand.Name = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Key_Argument_Is_Null()
        {
            // Arrange
            editGameRightCommand.Key = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Key_Argument_Is_Empty()
        {
            // Arrange
            editGameRightCommand.Key = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Description_Argument_Is_Null()
        {
            // Arrange
            editGameRightCommand.Description = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("Description", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Description_Argument_Is_Empty()
        {
            // Arrange
            editGameRightCommand.Description = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("Description", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_GenresIds_Argument_Is_Null()
        {
            // Arrange
            editGameRightCommand.GenreIds = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_GenreIds_Argument_Is_Empty()
        {
            // Arrange
            editGameRightCommand.GenreIds = Enumerable.Empty<Int32>().ToArray();

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_PlatformTypeIds_Argument_Is_Null()
        {
            // Arrange
            editGameRightCommand.PlatformTypeIds = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_PlatformTypeIds_Argument_Is_Empty()
        {
            // Arrange
            editGameRightCommand.PlatformTypeIds = Enumerable.Empty<Int32>().ToArray();

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_With_Existing_Key()
        {
            // Arrange
            editGameRightCommand.Key = "witcher-3";

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_With_Non_Exising_Genre()
        {
            // Arrange
            editGameRightCommand.GenreIds = new[] { 1, 5 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_With_Non_Exising_PlatformType()
        {
            // Arrange
            editGameRightCommand.PlatformTypeIds = new[] { 1, 5 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(editGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_With_Right_Data()
        {
            // Arrange
            // Act
            commandHandler.Execute(editGameRightCommand);

            // Assert
            gameRepositoryMock.Verify(x => x.Update(It.IsAny<Game>()), Times.Once);
            unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }

        #endregion

        #region Delete_Tests

        [TestMethod]
        public void Delete_Game_Key_Argument_Is_Null()
        {
            // Arrange
            var deleteGameCommand = new DeleteGameCommand { Key = null };

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                commandHandler.Execute(deleteGameCommand));

            // Assert
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Delete_Game_Key_Argument_Is_Empty()
        {
            // Arrange
            var deleteGameCommand = new DeleteGameCommand { Key = String.Empty };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(deleteGameCommand));

            // Assert
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Delete_Game_Key_Argument_Doesnt_Match_Exising_Game()
        {
            // Arrange
            var deleteGameCommand = new DeleteGameCommand() { Key = "not-existing-game" };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(deleteGameCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Delete_Game_With_Right_Data()
        {
            // Arrange
            var deleteGameCommand = new DeleteGameCommand { Key = "dota-2" };

            // Act
            commandHandler.Execute(deleteGameCommand);

            // Assert
            unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }
        #endregion

        #region Get_Queries_Tests

        [TestMethod]
        public void Get_All_Games()
        {
            // Arrange
            var getAllGamesQuery = new GetAllGamesQuery();

            // Act
            var result = queryHandler.Retrieve(getAllGamesQuery);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetGameById_Id_Argument_Is_Zero()
        {
            // Arrange
            var getGameById = new GetGameByIdQuery { Id = 0 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                queryHandler.Retrieve(getGameById));

            // Assert
            gameRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void GetGameById_Id_Argument_Lower_Than_Zero()
        {
            // Arrange
            var getGameById = new GetGameByIdQuery { Id = -1 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                queryHandler.Retrieve(getGameById));

            // Assert
            gameRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void GetGameById_Right_Data()
        {
            // Arrange
            var getGameById = new GetGameByIdQuery { Id = 1 };

            // Act
            var result = queryHandler.Retrieve(getGameById);

            // Assert
            gameRepositoryMock.Verify(x => x.Get(It.Is<Int32>(i => i == 1)), Times.Once());
            Assert.AreEqual("Dota 2", result.Name);
        }

        [TestMethod]
        public void GetGamesByGenre_Id_Argument_Lower_Than_Zero()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery { Id = -1 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                queryHandler.Retrieve(getGamesByGenre));

            // Assert
            unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByGenre_Name_Argument_Is_Empty()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery { Name = String.Empty };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                queryHandler.Retrieve(getGamesByGenre));

            // Assert
            unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByGenre_Id_Argument_Is_Zero_And_Name_Argument_Is_Null()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery();

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                queryHandler.Retrieve(getGamesByGenre));

            // Assert
            unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Id, Name", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByGenre_Id_Argument_Doesnt_Match_Existing_Genre()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery { Id = 5 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                queryHandler.Retrieve(getGamesByGenre));

            // Assert
            genreRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Once);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByGenre_Name_Argument_Doesnt_Match_Existing_Genre()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery { Name = "notExisingGenre" };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                queryHandler.Retrieve(getGamesByGenre));

            // Assert
            genreRepositoryMock.Verify(x => x.GetSingle(It.IsAny<Expression<Func<Genre, Boolean>>>()), Times.Once);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByGenre_Id_Argument_Is_Used()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery { Id = 2 };

            // Act
            var result = queryHandler.Retrieve(getGamesByGenre);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetGamesByGenre_Name_Argument_Is_Used()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery() { Name = "RTS" };

            // Act
            var result = queryHandler.Retrieve(getGamesByGenre);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetGamesByPlatformTypes_Ids_Argument_Contains_Zero()
        {
            // Arrange
            var getGamesByPlatformTypes = new GetGamesByPlatformTypesQuery { Ids = new[] { 0 } };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                queryHandler.Retrieve(getGamesByPlatformTypes));

            // Assert
            unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Ids", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByPlatformTypes_Ids_Argument_Contains_Negative_Number()
        {
            // Arrange
            var getGamesByPlatformTypes = new GetGamesByPlatformTypesQuery { Ids = new[] { -1 } };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                queryHandler.Retrieve(getGamesByPlatformTypes));

            // Assert
            unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Ids", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByPlatformTypes_Names_Argument_Contains_Empty_Strings()
        {
            // Arrange
            var getGamesByPlatformTypes = new GetGamesByPlatformTypesQuery { Names = new[] { String.Empty } };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                queryHandler.Retrieve(getGamesByPlatformTypes));

            // Assert
            unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Names", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByPlatformTypes_Ids_And_Names_Arguments_Are_Null()
        {
            // Arrange
            var getGamesByPlatformTypes = new GetGamesByPlatformTypesQuery();

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                queryHandler.Retrieve(getGamesByPlatformTypes));

            // Assert
            unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Ids, Names", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByPlatformTypes_Ids_Argument_Used()
        {
            // Arrange
            var getGamesByPlatformTypes = new GetGamesByPlatformTypesQuery { Ids = new[] { 1 } };

            // Act
            var result = queryHandler.Retrieve(getGamesByPlatformTypes);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetGamesByPlatformTypes_Names_Argument_Used()
        {
            // Arrange
            var getGamesByPlatformTypes = new GetGamesByPlatformTypesQuery { Names = new[] { "Web" } };

            // Act
            var result = queryHandler.Retrieve(getGamesByPlatformTypes);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetGameByKey_Key_Argument_Is_Null()
        {
            // Arrange
            var getGameByKey = new GetGameByKeyQuery();

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                queryHandler.Retrieve(getGameByKey));

            // Assert
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void GetGameByKey_Key_Argument_Is_Empty()
        {
            // Arrange
            var getGameByKey = new GetGameByKeyQuery { Key = String.Empty };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                queryHandler.Retrieve(getGameByKey));

            // Assert
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void GetGameByKey_Right_Data()
        {
            // Arrange
            var getGameByKey = new GetGameByKeyQuery { Key = "dota-2" };

            // Act
            var result = queryHandler.Retrieve(getGameByKey);

            // Assert
            gameRepositoryMock.Verify(x => x.GetSingle(It.IsAny<Expression<Func<Game, Boolean>>>()), Times.Once);
            Assert.AreEqual("Dota 2", result.Name);
        }
        #endregion
    }
}
