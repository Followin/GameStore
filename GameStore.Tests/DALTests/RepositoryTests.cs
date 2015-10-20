using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using GameStore.DAL.Abstract;
using GameStore.DAL.EF;
using GameStore.DAL.Repositories;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.DALTests
{
    public class TestClass : Entity<Int32>
    { }

    [TestClass]
    public class RepositoryTests 
    {
        private Mock<IContext> dbContext;
        private Mock<IDbSet<TestClass>> testClassSetMock;
        private GenericRepository<TestClass> testGenericRepository;
        private GameStoreUnitOfWork unitOfWork;
            
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            
        }

        [TestInitialize]
        public void TestInitialize()
        {
            dbContext = new Mock<IContext>();
            testClassSetMock = new Mock<IDbSet<TestClass>>();
            testClassSetMock.Setup(x => x.Find(It.IsAny<Int32>())).Returns(
                (Object[] i) => new TestClass { Id = (Int32)i[0]});
            dbContext.Setup(x => x.Set<TestClass>()).Returns(testClassSetMock.Object);
            testGenericRepository = new GenericRepository<TestClass>(dbContext.Object);
            unitOfWork = new GameStoreUnitOfWork(dbContext.Object);
        }

        

        [TestMethod]
        public void Add_Calls_Add_Method_On_Context()
        {
            //Arrange

            //Act
            testGenericRepository.Add(new TestClass());
            
            //Assert
            testClassSetMock.Verify(x => x.Add(It.IsAny<TestClass>()), Times.Once);

        }

        [TestMethod]
        public void Update_Calls_Entry_Method_On_Context()
        {
            //Arrange

            //Act
            testGenericRepository.Update(new TestClass());

            //Assert
            dbContext.Verify(x => x.SetModified(It.IsAny<TestClass>()), Times.Once);
        }

        [TestMethod]
        public void Delete_Calls_Remove_Method_On_Context()
        {
            //Arrange


            //Act
            testGenericRepository.Delete(1);

            //Assert
            testClassSetMock.Verify(x => x.Remove(It.Is<TestClass>(t => t.Id == 1)), Times.Once);
        }

        [TestMethod]
        public void Get_With_Id_Parameter_Calls_Find()
        {
            //Act
            testGenericRepository.Get(1);

            //Assert
            testClassSetMock.Verify(x => x.Find(It.Is<Int32>(_ => _ == 1)), Times.Once);
        }


        [TestMethod]
        public void GetSingle_Returns_Item_Matching_Predicate()
        {
            //Arrange
            var items = new List<TestClass> {new TestClass {Id = 1}, new TestClass {Id = 2}}.AsQueryable();
            testClassSetMock.Setup(x => x.ElementType).Returns(items.ElementType);
            testClassSetMock.Setup(x => x.Expression).Returns(items.Expression);
            testClassSetMock.Setup(x => x.Provider).Returns(items.Provider);
            testClassSetMock.Setup(x => x.GetEnumerator()).Returns(items.GetEnumerator);

            //Act
            var result = testGenericRepository.GetSingle(t => t.Id == 2);

            //Assert
            Assert.AreEqual(2, result.Id);
        }

        [TestMethod]
        public void Get_With_Predicate_Parameter_Returns_Items_Matching_Predicate()
        {
            var items = new List<TestClass> { new TestClass { Id = 1 }, new TestClass { Id = 1 }, new TestClass { Id = 2} }.AsQueryable();
            testClassSetMock.Setup(x => x.ElementType).Returns(items.ElementType);
            testClassSetMock.Setup(x => x.Expression).Returns(items.Expression);
            testClassSetMock.Setup(x => x.Provider).Returns(items.Provider);
            testClassSetMock.Setup(x => x.GetEnumerator()).Returns(items.GetEnumerator);

            //Act
            var result = testGenericRepository.Get(t => t.Id == 1);

            //Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void Get_With_No_Parameters_Returns_Whole_Object()
        {
            //Act
            var result = testGenericRepository.Get();

            //Assert
            Assert.AreEqual(testClassSetMock.Object, result);
        }


        [TestMethod]
        public void Save_Method_Calls_Save_Changes_Method_On_Context()
        {
            //Act
            unitOfWork.Save();

            //Assert
            dbContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Dispose_Method_Calls_Dispose_Method_On_Context()
        {
            //Act
            unitOfWork.Dispose();

            //Assert
            dbContext.Verify(x => x.Dispose(), Times.Once);
        }
        
        

    }
}
