using System;
using System.Linq;
using System.Linq.Expressions;
using GameStore.BLL.CommandHandlers;
using GameStore.BLL.CommandHandlers.Game;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Game;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryHandlers;
using GameStore.BLL.QueryHandlers.Game;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;
using GameStore.Static;
using GameStore.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;

namespace GameStore.Tests.BLLTests
{
    [TestClass]
    public class GameHandlerTests
    {
        private CreateGameCommandHandler _addGameCommandHandler;
        private EditGameCommandHandler _editGameCommandHandler;
        private DeleteGameCommandHandler _deleteGameCommandHandler;
        private Mock<IGameRepository> _gameRepositoryMock;
        private Mock<IGenreRepository> _genreRepositoryMock;
        private Mock<IPlatformTypeRepository> _platformTypeRepositoryMock;
        private Mock<IPublisherRepository> _publisherRepositoryMock;
        private Mock<IGameStoreUnitOfWork> _unitOfWorkMock;
        private CreateGameCommand _newGameRightCommand;
        private EditGameCommand _editGameRightCommand;
        private Mock<ILogger> _logger;
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
                NameEn = "RTS",
                ChildGenres = new Genre[0]
            };

            var strategy = new Genre
            {
                Id = 2,
                NameEn = "Strategy",
                ChildGenres = new[] { rts }
            };

            rts.ParentGenre = strategy;
            rts.ParentGenreId = 2;
            var genres = new[] { rts, strategy };
            _genreRepositoryMock = new Mock<IGenreRepository>();
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
            _platformTypeRepositoryMock = new Mock<IPlatformTypeRepository>();
            _platformTypeRepositoryMock.Setup(x => x.Get()).Returns(platformTypes);
            _platformTypeRepositoryMock.Setup(x => x.Get(It.IsAny<Int32>())).Returns(
                (Int32 i) => platformTypes.FirstOrDefault(g => g.Id == i));
            _platformTypeRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<PlatformType, Boolean>>>())).Returns(
                (Expression<Func<PlatformType, Boolean>> predicate) => platformTypes.FirstOrDefault(predicate.Compile()));
            _platformTypeRepositoryMock.Setup(x => x.Get(
                It.IsAny<Expression<Func<PlatformType, Boolean>>>())).Returns(
                    (Expression<Func<PlatformType, Boolean>> predicate,
                    Expression<Func<PlatformType, object>> orderBy) => platformTypes.Where(predicate.Compile()));

            var valve = new Publisher
            {
                Id = 1,
                CompanyName = "Valve",
                Description = "Greed Gaben's company",
                HomePage = "http://www.valvesoftware.com/",
            };
            var cdProject = new Publisher
            {
                Id = 2,
                CompanyName = "CD Project",
                Description = "Poland private game developing company",
                HomePage = "https://www.cdprojekt.com/"
            };
            var publishers = new[] { valve, cdProject };
            _publisherRepositoryMock = new Mock<IPublisherRepository>();
            _publisherRepositoryMock.Setup(x => x.Get()).Returns(publishers);
            _publisherRepositoryMock.Setup(x => x.Get(It.IsAny<Int32>())).Returns(
                (Int32 i) => publishers.FirstOrDefault(p => p.Id == i));

            _newGameRightCommand = new CreateGameCommand
            {
                Name = "GTA 5",
                DescriptionEn = "5 part",
                Key = "gta-5",
                GenreIds = new[] { 1 },
                PlatformTypeIds = new[] { 1 },
                Price = 150,
                PublicationDate = new DateTime(2015, 01,01),
                UnitsInStock = 20,
                Discontinued = true,
                PublisherId = 1
            };

            _editGameRightCommand = new EditGameCommand
            {
                Id = 1,
                Name = "New name",
                DescriptionEn = "New description",
                Key = "new-key",
                GenreIds = new[] { 2 },
                PlatformTypeIds = new[] { 2 },
                Price = 100,
                UnitsInStock = 15,
                Discontinued = false,
                PublisherId = 2
            };

            _dota = new Game
            {
                Id = 1,
                Name = "Dota 2",
                DescriptionEn = "Just try it",
                Key = "dota-2",
                Genres = new[] { rts },
                PlatformTypes = new[] { desktop, web },
                Publisher = valve,
                PublisherId = 1,
                Discontinued = false,
                UnitsInStock = 50,
                PublicationDate = new DateTime(2015, 01, 01),
                Price = 200
            };

            _witcher = new Game
            {
                Id = 2,
                Name = "Witcher 3",
                DescriptionEn = "3d part of trilogy",
                Key = "witcher-3",
                Genres = new[] { strategy },
                PlatformTypes = new[] { desktop },
                Publisher = cdProject,
                Discontinued = false,
                UnitsInStock = 50,
                Price = 150
            };
            _games = new[] { _dota, _witcher };
            _gameRepositoryMock = new Mock<IGameRepository>();
            _gameRepositoryMock.Setup(x => x.Get()).Returns(_games);
            _gameRepositoryMock.Setup(x => x.Get(It.Is<Int32>(i => i == 1))).Returns(_dota);
            _gameRepositoryMock.Setup(x => x.Get(It.Is<Int32>(i => i == 2))).Returns(_witcher);
            _gameRepositoryMock.Setup(x => x.Get(
                It.IsAny<Expression<Func<Game, Boolean>>>())).Returns(
                    (Expression<Func<Game, Boolean>> predicate) => _games.Where(predicate.Compile()));
            _gameRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<Game, Boolean>>>())).Returns(
                (Expression<Func<Game, Boolean>> predicate) => _games.FirstOrDefault(predicate.Compile()));
            _gameRepositoryMock.Setup(x => x.Get(
                It.IsAny<Expression<Func<Game, Boolean>>>(),
                It.IsAny<GamesOrderType>(),
                It.IsAny<Int32?>(),
                It.IsAny<Int32?>())).Returns(
                    (Expression<Func<Game, Boolean>> predicate, GamesOrderType order, Int32? skip, Int32? take) =>
                    _games.Where(predicate.Compile()));
            _gameRepositoryMock.Setup(x => x.GetCount(It.IsAny<Expression<Func<Game, Boolean>>>()))
                               .Returns((Expression<Func<Game, Boolean>> predicate) => _games.Count(predicate.Compile()));

            _unitOfWorkMock = new Mock<IGameStoreUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Games).Returns(_gameRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.Genres).Returns(_genreRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.PlatformTypes).Returns(_platformTypeRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.Publishers).Returns(_publisherRepositoryMock.Object);

            _logger = new Mock<ILogger>();
            _addGameCommandHandler = new CreateGameCommandHandler(_unitOfWorkMock.Object, _logger.Object);
            _editGameCommandHandler = new EditGameCommandHandler(_unitOfWorkMock.Object, _logger.Object);
            _deleteGameCommandHandler = new DeleteGameCommandHandler(_unitOfWorkMock.Object, _logger.Object);
        }

        #region Create_Tests
        [TestMethod]
        public void Create_Game_Name_Argument_Is_Null()
        {
            // Arrange
            _newGameRightCommand.Name = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _addGameCommandHandler.Execute(_newGameRightCommand));

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
                _addGameCommandHandler.Execute(_newGameRightCommand));

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
                _addGameCommandHandler.Execute(_newGameRightCommand));

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
                _addGameCommandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_Description_Argument_Is_Null()
        {
            // Arrange
            _newGameRightCommand.DescriptionEn = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _addGameCommandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("DescriptionEn", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_Description_Argument_Is_Empty()
        {
            // Arrange
            _newGameRightCommand.DescriptionEn = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _addGameCommandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("DescriptionEn", result.ParamName);
        }



        [TestMethod]
        public void Create_Game_PlatformTypeIds_Argument_Is_Null()
        {
            // Arrange
            _newGameRightCommand.PlatformTypeIds = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _addGameCommandHandler.Execute(_newGameRightCommand));

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
                _addGameCommandHandler.Execute(_newGameRightCommand));

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
                _addGameCommandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_With_Non_Exising_Genre()
        {
            // Arrange
            _newGameRightCommand.GenreIds = new[] { 1, 5 };

            // Act
            var result = ExceptionAssert.Throws<EntityNotFoundException>(() =>
                _addGameCommandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_With_Non_Exising_PlatformType()
        {
            // Arrange
            _newGameRightCommand.PlatformTypeIds = new[] { 1, 5 };

            // Act
            var result = ExceptionAssert.Throws<EntityNotFoundException>(() =>
                _addGameCommandHandler.Execute(_newGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_With_No_Price_Argument()
        {
            // Arrange
            _newGameRightCommand.Price = 0;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _addGameCommandHandler.Execute(_newGameRightCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Price", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_Price_Argument_Is_Negative()
        {
            // Arrange
            _newGameRightCommand.Price = -1;
            
            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _addGameCommandHandler.Execute(_newGameRightCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Price", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_UnitsInStock_Argument_Is_Negative()
        {
            // Arrange
            _newGameRightCommand.UnitsInStock = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _addGameCommandHandler.Execute(_newGameRightCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("UnitsInStock", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_PublisherId_Argument_Is_Zero()
        {
            // Arrange
            _newGameRightCommand.PublisherId = 0;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _addGameCommandHandler.Execute(_newGameRightCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            _unitOfWorkMock.Verify(x => x.Publishers, Times.Never);
            Assert.AreEqual("PublisherId", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_PublisherId_Argument_Is_Negative()
        {
            // Arrange
            _newGameRightCommand.PublisherId = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _addGameCommandHandler.Execute(_newGameRightCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            _unitOfWorkMock.Verify(x => x.Publishers, Times.Never);
            Assert.AreEqual("PublisherId", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_PublisherId_Argument_Doesnt_Match_Existing_Publisher()
        {
            // Arrange
            _newGameRightCommand.PublisherId = 100;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _addGameCommandHandler.Execute(_newGameRightCommand));

            // Assert
            _gameRepositoryMock.Verify(x => x.Add(It.IsAny<Game>()), Times.Never);
            Assert.AreEqual("PublisherId", result.ParamName);
        }

        [TestMethod]
        public void Create_Game_With_Right_Data()
        {
            // Arrange
            // Act
            _addGameCommandHandler.Execute(_newGameRightCommand);

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
                _editGameCommandHandler.Execute(_editGameRightCommand));

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
                _editGameCommandHandler.Execute(_editGameRightCommand));

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
                _editGameCommandHandler.Execute(_editGameRightCommand));

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
                _editGameCommandHandler.Execute(_editGameRightCommand));

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
                _editGameCommandHandler.Execute(_editGameRightCommand));

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
                _editGameCommandHandler.Execute(_editGameRightCommand));

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
                _editGameCommandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Description_Argument_Is_Null()
        {
            // Arrange
            _editGameRightCommand.DescriptionEn = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _editGameCommandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("DescriptionEn", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Description_Argument_Is_Empty()
        {
            // Arrange
            _editGameRightCommand.DescriptionEn = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _editGameCommandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("DescriptionEn", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_GenresIds_Argument_Is_Null()
        {
            // Arrange
            _editGameRightCommand.GenreIds = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _editGameCommandHandler.Execute(_editGameRightCommand));

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
                _editGameCommandHandler.Execute(_editGameRightCommand));

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
                _editGameCommandHandler.Execute(_editGameRightCommand));

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
                _editGameCommandHandler.Execute(_editGameRightCommand));

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
                _editGameCommandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_With_Non_Exising_Genre()
        {
            // Arrange
            _editGameRightCommand.GenreIds = new[] { 1, 5 };

            // Act
            var result = ExceptionAssert.Throws<EntityNotFoundException>(() =>
                _editGameCommandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("GenreIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_With_Non_Exising_PlatformType()
        {
            // Arrange
            _editGameRightCommand.PlatformTypeIds = new[] { 1, 5 };

            // Act
            var result = ExceptionAssert.Throws<EntityNotFoundException>(() =>
                _editGameCommandHandler.Execute(_editGameRightCommand));

            // Assert
            Assert.AreEqual("PlatformTypeIds", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_With_No_Price_Argument()
        {
            // Arrange
            _editGameRightCommand.Price = 0;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _editGameCommandHandler.Execute(_editGameRightCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Price", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_Price_Argument_Is_Negative()
        {
            // Arrange
            _editGameRightCommand.Price = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _editGameCommandHandler.Execute(_editGameRightCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Price", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_UnitsInStock_Argument_Is_Negative()
        {
            // Arrange
            _editGameRightCommand.UnitsInStock = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _editGameCommandHandler.Execute(_editGameRightCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("UnitsInStock", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_PublisherId_Argument_Is_Zero()
        {
            // Arrange
            _editGameRightCommand.PublisherId = 0;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _editGameCommandHandler.Execute(_editGameRightCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            _unitOfWorkMock.Verify(x => x.Publishers, Times.Never);
            Assert.AreEqual("PublisherId", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_PublisherId_Argument_Is_Negative()
        {
            // Arrange
            _editGameRightCommand.PublisherId = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _editGameCommandHandler.Execute(_editGameRightCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            _unitOfWorkMock.Verify(x => x.Publishers, Times.Never);
            Assert.AreEqual("PublisherId", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_PublisherId_Argument_Doesnt_Match_Existing_Publisher()
        {
            // Arrange
            _editGameRightCommand.PublisherId = 100;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _editGameCommandHandler.Execute(_editGameRightCommand));

            // Assert
            _gameRepositoryMock.Verify(x => x.Update(It.IsAny<Game>()), Times.Never);
            Assert.AreEqual("PublisherId", result.ParamName);
        }

        [TestMethod]
        public void Edit_Game_With_Right_Data()
        {
            // Arrange
            // Act
            _editGameCommandHandler.Execute(_editGameRightCommand);

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
                _deleteGameCommandHandler.Execute(deleteGameCommand));

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
                _deleteGameCommandHandler.Execute(deleteGameCommand));

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
                _deleteGameCommandHandler.Execute(deleteGameCommand));

            // Assert
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void Delete_Game_With_Right_Data()
        {
            // Arrange
            var deleteGameCommand = new DeleteGameCommand { Key = "dota-2" };

            // Act
            _deleteGameCommandHandler.Execute(deleteGameCommand);

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
            var getAllGamesQueryHandler = new GetAllGamesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = getAllGamesQueryHandler.Retrieve(getAllGamesQuery);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetGameById_Id_Argument_Is_Zero()
        {
            // Arrange
            var getGameById = new GetGameByIdQuery { Id = 0 };
            var getGameByIdHandler = new GetGameByIdQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                getGameByIdHandler.Retrieve(getGameById));

            // Assert
            _gameRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void GetGameById_Id_Argument_Lower_Than_Zero()
        {
            // Arrange
            var getGameById = new GetGameByIdQuery { Id = -1 };
            var getGameByIdHandler = new GetGameByIdQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                getGameByIdHandler.Retrieve(getGameById));

            // Assert
            _gameRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void GetGameById_Right_Data()
        {
            // Arrange
            var getGameById = new GetGameByIdQuery { Id = 1 };
            var getGameByIdHandler = new GetGameByIdQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = getGameByIdHandler.Retrieve(getGameById);

            // Assert
            _gameRepositoryMock.Verify(x => x.Get(It.Is<Int32>(i => i == 1)), Times.Once());
            Assert.AreEqual("Dota 2", result.Name);
        }

        [TestMethod]
        public void GetGamesByGenre_Id_Argument_Lower_Than_Zero()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery { Id = -1 };
            var getGamesByGenreHandler = new GetGamesByGenreQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                getGamesByGenreHandler.Retrieve(getGamesByGenre));

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
            var getGamesByGenreHandler = new GetGamesByGenreQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                getGamesByGenreHandler.Retrieve(getGamesByGenre));

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
            var getGamesByGenreHandler = new GetGamesByGenreQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                getGamesByGenreHandler.Retrieve(getGamesByGenre));

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
            var getGamesByGenreHandler = new GetGamesByGenreQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                getGamesByGenreHandler.Retrieve(getGamesByGenre));

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
            var getGamesByGenreHandler = new GetGamesByGenreQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = ExceptionAssert.Throws<EntityNotFoundException>(() =>
                getGamesByGenreHandler.Retrieve(getGamesByGenre));

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
            var getGamesByGenreHandler = new GetGamesByGenreQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = getGamesByGenreHandler.Retrieve(getGamesByGenre);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetGamesByGenre_Name_Argument_Is_Used()
        {
            // Arrange
            var getGamesByGenre = new GetGamesByGenreQuery() { Name = "RTS" };
            var getGamesByGenreHandler = new GetGamesByGenreQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = getGamesByGenreHandler.Retrieve(getGamesByGenre);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetGamesByPlatformTypes_Ids_Argument_Contains_Zero()
        {
            // Arrange
            var getGamesByPlatformTypes = new GetGamesByPlatformTypesQuery { Ids = new[] { 0 } };
            var getGamesByPlatformTypesHandler = new GetGamesByPlatformTypesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                getGamesByPlatformTypesHandler.Retrieve(getGamesByPlatformTypes));

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
            var getGamesByPlatformTypesHandler = new GetGamesByPlatformTypesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                getGamesByPlatformTypesHandler.Retrieve(getGamesByPlatformTypes));

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
            var getGamesByPlatformTypesHandler = new GetGamesByPlatformTypesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                getGamesByPlatformTypesHandler.Retrieve(getGamesByPlatformTypes));

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
            var getGamesByPlatformTypesHandler = new GetGamesByPlatformTypesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                getGamesByPlatformTypesHandler.Retrieve(getGamesByPlatformTypes));

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
            var getGamesByPlatformTypesHandler = new GetGamesByPlatformTypesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = getGamesByPlatformTypesHandler.Retrieve(getGamesByPlatformTypes);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetGamesByPlatformTypes_Names_Argument_Used()
        {
            // Arrange
            var getGamesByPlatformTypes = new GetGamesByPlatformTypesQuery { Names = new[] { "Web" } };
            var getGamesByPlatformTypesHandler = new GetGamesByPlatformTypesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = getGamesByPlatformTypesHandler.Retrieve(getGamesByPlatformTypes);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetGameByKey_Key_Argument_Is_Null()
        {
            // Arrange
            var getGameByKey = new GetGameByKeyQuery();
            var getGameByKeyQueryHandler = new GetGameByKeyQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                getGameByKeyQueryHandler.Retrieve(getGameByKey));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void GetGameByKey_Key_Argument_Is_Empty()
        {
            // Arrange
            var getGameByKey = new GetGameByKeyQuery { Key = String.Empty };
            var getGameByKeyQueryHandler = new GetGameByKeyQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                getGameByKeyQueryHandler.Retrieve(getGameByKey));

            // Assert
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void GetGameByKey_Right_Data()
        {
            // Arrange
            var getGameByKey = new GetGameByKeyQuery { Key = "dota-2" };
            var getGameByKeyQueryHandler = new GetGameByKeyQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = getGameByKeyQueryHandler.Retrieve(getGameByKey);

            // Assert
            _gameRepositoryMock.Verify(x => x.GetSingle(It.IsAny<Expression<Func<Game, Boolean>>>()), Times.Once);
            Assert.AreEqual("Dota 2", result.Name);
        }

        [TestMethod]
        public void GetAllGamesQuery_Right_Data()
        {
            // Arrange
            var getGamesQuery = new GetAllGamesQuery();
            var getAllGamesQueryHandler = new GetAllGamesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            // Act
            var result = getAllGamesQueryHandler.Retrieve(getGamesQuery);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetGamesQuery_Right_Data()
        {
            //Arrange
            var getGamesQuery = new GetGamesQuery
            {
                Name = "Dota 2",
                GenreIds = new[] {1},
                MaxPrice = 300,
                MinDate = new DateTime(2014, 01, 01),
                OrderBy = GamesOrderType.PriceAsc,
                MinPrice = 50,
                Number = 5,
                PlatformTypeIds = new[] {1},
                PublisherIds = new[] {1},
                Skip = 0
            };
            var getGamesQueryHandler = new GetGamesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            //Act
            var result = getGamesQueryHandler.Retrieve(getGamesQuery);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetGamesQuery_Using_Name()
        {
            //Arrange
            var getGamesQuery = new GetGamesQuery
            {
                Name = "Dota 2"
            };
            var getGamesQueryHandler = new GetGamesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            //Act
            var result = getGamesQueryHandler.Retrieve(getGamesQuery);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetGamesQuery_Using_GenreIds()
        {
            //Arrange
            var getGamesQuery = new GetGamesQuery
            {
                GenreIds = new[] { 1 }
            };
            var getGamesQueryHandler = new GetGamesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            //Act
            var result = getGamesQueryHandler.Retrieve(getGamesQuery);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetGamesQuery_Using_PlatformTypeIds()
        {
            //Arrange
            var getGamesQuery = new GetGamesQuery
            {
                PlatformTypeIds = new[] { 1 }
            };
            var getGamesQueryHandler = new GetGamesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            //Act
            var result = getGamesQueryHandler.Retrieve(getGamesQuery);

            //Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetGamesQuery_Using_MinDate()
        {
            //Arrange
            var getGamesQuery = new GetGamesQuery
            {
                MinDate = new DateTime(2014, 1, 1)
            };
            var getGamesQueryHandler = new GetGamesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            //Act
            var result = getGamesQueryHandler.Retrieve(getGamesQuery);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetGamesQuery_Using_MinPrice()
        {
            //Arrange
            var getGamesQuery = new GetGamesQuery
            {
                MinPrice = 50
            };
            var getGamesQueryHandler = new GetGamesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            //Act
            var result = getGamesQueryHandler.Retrieve(getGamesQuery);

            //Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetGamesQuery_Using_MaxPrice()
        {
            //Arrange
            var getGamesQuery = new GetGamesQuery
            {
                MaxPrice = 300
            };
            var getGamesQueryHandler = new GetGamesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            //Act
            var result = getGamesQueryHandler.Retrieve(getGamesQuery);

            //Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetGamesQuery_Using_PublisherIds()
        {
            //Arrange
            var getGamesQuery = new GetGamesQuery
            {
                PublisherIds = new[] { 1 }
            };
            var getGamesQueryHandler = new GetGamesQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            //Act
            var result = getGamesQueryHandler.Retrieve(getGamesQuery);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetGamesCountQuery()
        {
            //Arrange
            var getGamesCountQuery = new GetGamesCountQuery();
            var getGamesCountQueryHandler = new GetGamesCountQueryHandler(_unitOfWorkMock.Object, _logger.Object);

            //Act
            var result = getGamesCountQueryHandler.Retrieve(getGamesCountQuery);

            //Assert
            Assert.AreEqual(2, result.Count);
        }
        #endregion
    }
}
