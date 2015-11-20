using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.Queries.Genre;
using GameStore.BLL.QueryHandlers.Genre;
using GameStore.DAL.Abstract;
using GameStore.DAL.Abstract.Repositories;
using GameStore.Domain.Entities;
using GameStore.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;

namespace GameStore.Tests.BLLTests
{
    [TestClass]
    public class GenreTests
    {
        private Mock<IGenreRepository> _genreRepositoryMock;
        private Mock<IGameStoreUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger> _loggerMock;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            AutoMapperConfiguration.Configure();
        }

        [TestInitialize]
        public void TestInitialize()
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

            var genres = new[] {rts, strategy};
            _genreRepositoryMock = new Mock<IGenreRepository>();
            _genreRepositoryMock.Setup(x => x.Get()).Returns(genres);
            _genreRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(
                (int i) => genres.FirstOrDefault(g => g.Id == i));
            _genreRepositoryMock.Setup(x => x.GetFirst(It.IsAny<Expression<Func<Genre, bool>>>())).Returns(
                (Expression<Func<Genre, bool>> predicate) => genres.FirstOrDefault(predicate.Compile()));
            _genreRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Genre, bool>>>())).Returns(
                (Expression<Func<Genre, bool>> predicate) => genres.Where(predicate.Compile()));

            _unitOfWorkMock = new Mock<IGameStoreUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Genres).Returns(_genreRepositoryMock.Object);

            _loggerMock = new Mock<ILogger>();
        }

        [TestMethod]
        public void GetAllGenres_Returns_Only_Genre_With_No_Parent_With_Children()
        {
            // Arrange
            var getAllGenresQuery = new GetAllGenresQuery();
            var getAllGenresQueryHandler = new GetAllGenresQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object);

            // Act
            var result = getAllGenresQueryHandler.Retrieve(getAllGenresQuery);

            // Assert
            Assert.AreEqual(1, result.Count());
        }
    }

}
