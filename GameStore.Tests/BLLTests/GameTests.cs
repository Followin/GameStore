﻿using System;
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
        private GameCommandHandler _commandHandler;
        private GameQueryHandler _queryHandler;
        private Mock<IRepository<Game, Int32>> _gameRepositoryMock;
        private Mock<IRepository<Genre, Int32>> _genreRepositoryMock;
        private Mock<IRepository<PlatformType, Int32>> _platformTypeRepositoryMock;
        private Mock<IGameStoreUnitOfWork> _unitOfWorkMock;
        private CreateGameCommand _newGameRightCommand;
        private EditGameCommand _editGameRightCommand;
        private Game _dota;
        private Game _witcher;
        private Game[] _games;

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
            _genreRepositoryMock = new Mock<IRepository<Genre, int>>();
            _genreRepositoryMock.Setup(x => x.Get()).Returns(genres);
            _genreRepositoryMock.Setup(x => x.Get(It.IsAny<Int32>())).Returns(
                (Int32 i) => genres.FirstOrDefault(g => g.Id == i));
            _genreRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<Genre, Boolean>>>())).Returns(
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
            _platformTypeRepositoryMock = new Mock<IRepository<PlatformType, int>>();
            _platformTypeRepositoryMock.Setup(x => x.Get()).Returns(platformTypes);
            _platformTypeRepositoryMock.Setup(x => x.Get(It.IsAny<Int32>())).Returns(
                (Int32 i) => platformTypes.FirstOrDefault(g => g.Id == i));
            _platformTypeRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<PlatformType, Boolean>>>())).Returns(
                (Expression<Func<PlatformType, Boolean>> predicate) => platformTypes.FirstOrDefault(predicate.Compile()));
            _platformTypeRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<PlatformType, Boolean>>>())).Returns(
                (Expression<Func<PlatformType, Boolean>> predicate) => platformTypes.Where(predicate.Compile()));

            _newGameRightCommand = new CreateGameCommand
            {
                Name = "GTA 5",
                Description = "5 part",
                Key = "gta-5",
                GenreIds = new[] { 1 },
                PlatformTypeIds = new[] { 1 }
            };

            _editGameRightCommand = new EditGameCommand
            {
                Id = 1,
                Name = "New name",
                Description = "New description",
                Key = "new-key",
                GenreIds = new[] { 2 },
                PlatformTypeIds = new[] { 2 }
            };

            _dota = new Game
            {
                Id = 1,
                Name = "Dota 2",
                Description = "Just try it",
                Key = "dota-2",
                Genres = new[] { rts },
                PlatformTypes = new[] { desktop, web }
            };

            _witcher = new Game
            {
                Id = 2,
                Name = "Witcher 3",
                Description = "3d part of trilogy",
                Key = "witcher-3",
                Genres = new[] { strategy },
                PlatformTypes = new[] { desktop }
            };
            _games = new[] { _dota, _witcher };
            _gameRepositoryMock = new Mock<IRepository<Game, int>>();
            _gameRepositoryMock.Setup(x => x.Get()).Returns(_games);
            _gameRepositoryMock.Setup(x => x.Get(It.Is<Int32>(i => i == 1))).Returns(_dota);
            _gameRepositoryMock.Setup(x => x.Get(It.Is<Int32>(i => i == 2))).Returns(_witcher);
            _gameRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Game, Boolean>>>())).Returns(
                (Expression<Func<Game, Boolean>> predicate) => _games.Where(predicate.Compile()));
            _gameRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<Game, Boolean>>>())).Returns(
                (Expression<Func<Game, Boolean>> predicate) => _games.FirstOrDefault(predicate.Compile()));

            _unitOfWorkMock = new Mock<IGameStoreUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Games).Returns(_gameRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.Genres).Returns(_genreRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.PlatformTypes).Returns(_platformTypeRepositoryMock.Object);

            var logger = new Mock<ILogger>();
            _commandHandler = new GameCommandHandler(_unitOfWorkMock.Object, logger.Object);
            _queryHandler = new GameQueryHandler(_unitOfWorkMock.Object, logger.Object);
        }

        #region Create_Tests
        [TestMethod]
        public void Create_Game_Name_Argument_Is_Null()
        {
            // Arrange
            _newGameRightCommand.Name = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() => 
                _commandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_Name_Argument_Is_Empty()
        {
            // Arrange
            _newGameRightCommand.Name = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_Key_Argument_Is_Null()
        {
            // Arrange
            _newGameRightCommand.Key = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() => 
                _commandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_Key_Argument_Is_Empty()
        {
            // Arrange
            _newGameRightCommand.Key = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_Description_Argument_Is_Null()
        {
            // Arrange
            _newGameRightCommand.Description = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() => 
                _commandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("Description", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_Description_Argument_Is_Empty()
        {
            // Arrange
            _newGameRightCommand.Description = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("Description", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_GenresIds_Argument_Is_Null()
        {
            // Arrange
            _newGameRightCommand.GenreIds = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _commandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_GenreIds_Argument_Is_Empty()
        {
            // Arrange
            _newGameRightCommand.GenreIds = Enumerable.Empty<Int32>().ToArray();

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_PlatformTypeIds_Argument_Is_Null()
        {
            // Arrange
            _newGameRightCommand.PlatformTypeIds = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _commandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_PlatformTypeIds_Argument_Is_Empty()
        {
            // Arrange
            _newGameRightCommand.PlatformTypeIds = Enumerable.Empty<Int32>().ToArray();

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_With_Existing_Key()
        {
            // Arrange
            _newGameRightCommand.Key = "dota-2";
            _gameRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<Game, Boolean>>>()))
                .Returns((Expression<Func<Game, Boolean>> predicate) => predicate.Compile()(_dota) ? _dota : null);

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_With_Non_Exising_Genre()
        {
            // Arrange
            _newGameRightCommand.GenreIds = new[] { 1, 5 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_With_Non_Exising_PlatformType()
        {
            // Arrange
            _newGameRightCommand.PlatformTypeIds = new[] { 1, 5 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_With_Right_Data()
        {
            // Arrange
            // Act
            _commandHandler.Execute(_newGameRightCommand);

            // Assert
            _gameRepositoryMock.Verify(x => x.Add(It.IsAny<Game>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }

        #endregion

        #region Edit_Tests

        [TestMethod]
        public void Edit_Game_Id_Argument_Is_Zero()
        {
            // Arrange
            _editGameRightCommand.Id = 0;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            _gameRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Never());
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Id_Argument_Lower_Than_Zero()
        {
            // Arrange
            _editGameRightCommand.Id = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => 
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            _gameRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Never());
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Id_Argument_Doesnt_Match_Exising_Game()
        {
            // Arrange
            _editGameRightCommand.Id = 5;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Name_Argument_Is_Null()
        {
            // Arrange
            _editGameRightCommand.Name = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Name_Argument_Is_Empty()
        {
            // Arrange
            _editGameRightCommand.Name = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Key_Argument_Is_Null()
        {
            // Arrange
            _editGameRightCommand.Key = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Key_Argument_Is_Empty()
        {
            // Arrange
            _editGameRightCommand.Key = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Description_Argument_Is_Null()
        {
            // Arrange
            _editGameRightCommand.Description = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("Description", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Description_Argument_Is_Empty()
        {
            // Arrange
            _editGameRightCommand.Description = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("Description", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_GenresIds_Argument_Is_Null()
        {
            // Arrange
            _editGameRightCommand.GenreIds = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_GenreIds_Argument_Is_Empty()
        {
            // Arrange
            _editGameRightCommand.GenreIds = Enumerable.Empty<Int32>().ToArray();

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_PlatformTypeIds_Argument_Is_Null()
        {
            // Arrange
            _editGameRightCommand.PlatformTypeIds = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_PlatformTypeIds_Argument_Is_Empty()
        {
            // Arrange
            _editGameRightCommand.PlatformTypeIds = Enumerable.Empty<Int32>().ToArray();

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_With_Existing_Key()
        {
            // Arrange
            _editGameRightCommand.Key = "witcher-3";

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_With_Non_Exising_Genre()
        {
            // Arrange
            _editGameRightCommand.GenreIds = new[] { 1, 5 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_With_Non_Exising_PlatformType()
        {
            // Arrange
            _editGameRightCommand.PlatformTypeIds = new[] { 1, 5 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_With_Right_Data()
        {
            // Arrange
            // Act
            _commandHandler.Execute(_editGameRightCommand);

            // Assert
            _gameRepositoryMock.Verify(x => x.Update(It.IsAny<Game>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Save(), Times.Once);
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
                _commandHandler.Execute(deleteGameCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Delete_Game_Key_Argument_Is_Empty()
        {
            // Arrange
            var deleteGameCommand = new DeleteGameCommand { Key = String.Empty };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(deleteGameCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Delete_Game_Key_Argument_Doesnt_Match_Exising_Game()
        {
            // Arrange
            var deleteGameCommand = new DeleteGameCommand() { Key = "not-existing-game" };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(deleteGameCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Delete_Game_With_Right_Data()
        {
            // Arrange
            var deleteGameCommand = new DeleteGameCommand { Key = "dota-2" };

            // Act
            _commandHandler.Execute(deleteGameCommand);

            // Assert
            _unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }
        #endregion

        #region Get_Queries_Tests

        [TestMethod]
        public void Get_All_Games()
        {
            // Arrange
            var getAllGamesQuery = new GetAllGamesQuery();

            // Act
            var result = _queryHandler.Retrieve(getAllGamesQuery);

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
                _queryHandler.Retrieve(getGameById));

            // Assert
            _gameRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void GetGameById_Id_Argument_Lower_Than_Zero()
        {
            // Arrange
            var getGameById = new GetGameByIdQuery { Id = -1 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _queryHandler.Retrieve(getGameById));

            // Assert
            _gameRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void GetGameById_Right_Data()
        {
            // Arrange
            var getGameById = new GetGameByIdQuery { Id = 1 };

            // Act
            var result = _queryHandler.Retrieve(getGameById);

            // Assert
            _gameRepositoryMock.Verify(x => x.Get(It.Is<Int32>(i => i == 1)), Times.Once());
            Assert.AreEqual("Dota 2", result.Name);
        }

        [TestMethod]
        public void GetGamesByGenre_Id_Argument_Lower_Than_Zero()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery { Id = -1 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _queryHandler.Retrieve(getGamesByGenre));

            // Assert
            _unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByGenre_Name_Argument_Is_Empty()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery { Name = String.Empty };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _queryHandler.Retrieve(getGamesByGenre));

            // Assert
            _unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByGenre_Id_Argument_Is_Zero_And_Name_Argument_Is_Null()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery();

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _queryHandler.Retrieve(getGamesByGenre));

            // Assert
            _unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Id, Name", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByGenre_Id_Argument_Doesnt_Match_Existing_Genre()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery { Id = 5 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _queryHandler.Retrieve(getGamesByGenre));

            // Assert
            _genreRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByGenre_Name_Argument_Doesnt_Match_Existing_Genre()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery { Name = "notExisingGenre" };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _queryHandler.Retrieve(getGamesByGenre));

            // Assert
            _genreRepositoryMock.Verify(x => x.GetSingle(It.IsAny<Expression<Func<Genre, Boolean>>>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByGenre_Id_Argument_Is_Used()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery { Id = 2 };

            // Act
            var result = _queryHandler.Retrieve(getGamesByGenre);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetGamesByGenre_Name_Argument_Is_Used()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery() { Name = "RTS" };

            // Act
            var result = _queryHandler.Retrieve(getGamesByGenre);

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
                _queryHandler.Retrieve(getGamesByPlatformTypes));

            // Assert
            _unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Ids", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByPlatformTypes_Ids_Argument_Contains_Negative_Number()
        {
            // Arrange
            var getGamesByPlatformTypes = new GetGamesByPlatformTypesQuery { Ids = new[] { -1 } };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _queryHandler.Retrieve(getGamesByPlatformTypes));

            // Assert
            _unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Ids", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByPlatformTypes_Names_Argument_Contains_Empty_Strings()
        {
            // Arrange
            var getGamesByPlatformTypes = new GetGamesByPlatformTypesQuery { Names = new[] { String.Empty } };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _queryHandler.Retrieve(getGamesByPlatformTypes));

            // Assert
            _unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Names", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByPlatformTypes_Ids_And_Names_Arguments_Are_Null()
        {
            // Arrange
            var getGamesByPlatformTypes = new GetGamesByPlatformTypesQuery();

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _queryHandler.Retrieve(getGamesByPlatformTypes));

            // Assert
            _unitOfWorkMock.Verify(x => x.Genres, Times.Never);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Ids, Names", result.ParamName);
        }

        [TestMethod]
        public void GetGamesByPlatformTypes_Ids_Argument_Used()
        {
            // Arrange
            var getGamesByPlatformTypes = new GetGamesByPlatformTypesQuery { Ids = new[] { 1 } };

            // Act
            var result = _queryHandler.Retrieve(getGamesByPlatformTypes);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetGamesByPlatformTypes_Names_Argument_Used()
        {
            // Arrange
            var getGamesByPlatformTypes = new GetGamesByPlatformTypesQuery { Names = new[] { "Web" } };

            // Act
            var result = _queryHandler.Retrieve(getGamesByPlatformTypes);

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
                _queryHandler.Retrieve(getGameByKey));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void GetGameByKey_Key_Argument_Is_Empty()
        {
            // Arrange
            var getGameByKey = new GetGameByKeyQuery { Key = String.Empty };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _queryHandler.Retrieve(getGameByKey));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void GetGameByKey_Right_Data()
        {
            // Arrange
            var getGameByKey = new GetGameByKeyQuery { Key = "dota-2" };

            // Act
            var result = _queryHandler.Retrieve(getGameByKey);

            // Assert
            _gameRepositoryMock.Verify(x => x.GetSingle(It.IsAny<Expression<Func<Game, Boolean>>>()), Times.Once);
            Assert.AreEqual("Dota 2", result.Name);
        }
        #endregion
    }
}
