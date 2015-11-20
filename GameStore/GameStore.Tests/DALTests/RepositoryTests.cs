using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GameStore.DAL.Abstract;
using GameStore.DAL.Repositories;
using GameStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.DALTests
{
    public class TestClass : Entity<int>
    {
    }

    [TestClass]
    public class RepositoryTests
    {
        private Mock<IDbSet<TestClass>> _testClassSetMock;
        private GenericRepository<TestClass, int> _testGenericRepository;
        private GameStoreUnitOfWork _unitOfWork;
        private Mock<IEFContext> _efContext;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _efContext = new Mock<IEFContext>();
            _testClassSetMock = new Mock<IDbSet<TestClass>>();
            _testClassSetMock.Setup(x => x.Find(It.IsAny<int>())).Returns(
                (object[] i) => new TestClass { Id = (int)i[0] });
            _efContext.Setup(x => x.Set<TestClass>()).Returns(_testClassSetMock.Object);
            _testGenericRepository = new GenericRepository<TestClass, int>(_efContext.Object);
            _unitOfWork = new GameStoreUnitOfWork(_efContext.Object, null);
        }

        [TestMethod]
        public void Add_Calls_Add_Method_On_Context()
        {
            // Arrange

            // Act
            _testGenericRepository.Add(new TestClass());

            // Assert
            _testClassSetMock.Verify(x => x.Add(It.IsAny<TestClass>()), Times.Once);
        }

        [TestMethod]
        public void Update_Calls_Entry_Method_On_Context()
        {
            // Arrange

            // Act
            _testGenericRepository.Update(new TestClass());

            // Assert
            _efContext.Verify(x => x.SetModified(It.IsAny<TestClass>()), Times.Once);
        }

        [TestMethod]
        public void Delete_Calls_Remove_Method_On_Context()
        {
            // Arrange

            // Act
            _testGenericRepository.Delete(1);

            // Assert
            _testClassSetMock.Verify(x => x.Remove(It.Is<TestClass>(t => t.Id == 1)), Times.Once);
        }

        [TestMethod]
        public void Get_With_Id_Parameter_Calls_Find()
        {
            // Act
            _testGenericRepository.Get(1);

            // Assert
            _testClassSetMock.Verify(x => x.Find(It.Is<int>(i => i == 1)), Times.Once);
        }

        [TestMethod]
        public void GetSingle_Returns_Item_Matching_Predicate()
        {
            // Arrange
            var items = new List<TestClass> { new TestClass { Id = 1 }, new TestClass { Id = 2 } }.AsQueryable();
            _testClassSetMock.Setup(x => x.ElementType).Returns(items.ElementType);
            _testClassSetMock.Setup(x => x.Expression).Returns(items.Expression);
            _testClassSetMock.Setup(x => x.Provider).Returns(items.Provider);
            _testClassSetMock.Setup(x => x.GetEnumerator()).Returns(items.GetEnumerator);

            // Act
            var result = _testGenericRepository.GetFirst(t => t.Id == 2);

            // Assert
            Assert.AreEqual(2, result.Id);
        }

        [TestMethod]
        public void Get_With_Predicate_Parameter_Returns_Items_Matching_Predicate()
        {
            var items = new List<TestClass> { new TestClass { Id = 1 }, new TestClass { Id = 1 }, new TestClass { Id = 2 } }.AsQueryable();
            _testClassSetMock.Setup(x => x.ElementType).Returns(items.ElementType);
            _testClassSetMock.Setup(x => x.Expression).Returns(items.Expression);
            _testClassSetMock.Setup(x => x.Provider).Returns(items.Provider);
            _testClassSetMock.Setup(x => x.GetEnumerator()).Returns(items.GetEnumerator);

            // Act
            var result = _testGenericRepository.Get(t => t.Id == 1);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void Get_With_No_Parameters_Returns_All_Objects()
        {
            // Arrange
            var items = new List<TestClass> { new TestClass { Id = 1 }, new TestClass { Id = 1 }, new TestClass { Id = 2 } }.AsQueryable();
            _testClassSetMock.Setup(x => x.ElementType).Returns(items.ElementType);
            _testClassSetMock.Setup(x => x.Expression).Returns(items.Expression);
            _testClassSetMock.Setup(x => x.Provider).Returns(items.Provider);
            _testClassSetMock.Setup(x => x.GetEnumerator()).Returns(items.GetEnumerator);

            // Act
            var result = _testGenericRepository.Get();

            // Assert
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void Save_Method_Calls_Save_Changes_Method_On_Context()
        {
            // Act
            _unitOfWork.Save();

            // Assert
            _efContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Dispose_Method_Calls_Dispose_Method_On_Context()
        {
            // Act
            _unitOfWork.Dispose();

            // Assert
            _efContext.Verify(x => x.Dispose(), Times.Once);
        }


    }
}
