// Tools->Options->NuGet Package Manager->Package Sources->Remove offline office

using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using garderie.app2.Server.Controllers;
using garderie.app2.Server.Data;
using garderie.app2.Server.Models.Entities;

using Moq.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Xml;
using System.Threading;
using System.Reflection.Metadata;
using Azure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;
using System.Net.Sockets;

namespace garderie.app2.Server.Tests
{
    // Uncomment the following line to enable the tests in this class
    // [TestClass]
    public sealed class TestUsersControllerMock
    {
        public TestContext TestContext { get; set; }
        
        private UsersController GetDefaultUserController()
        {
            // Arrange
            var data = TestDataHelperUser.GetFakeUserList();
            var dataQueryable = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(dataQueryable.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(dataQueryable.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(dataQueryable.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => dataQueryable.GetEnumerator());

            mockDbSet.Setup(x => x.Add(It.IsAny<User>())).Callback<User>(t => data.Add(t));
            mockDbSet.Setup(x => x.Remove(It.IsAny<User>())).Callback<User>(t => data.Remove(t));
            mockDbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(x => (data as List<User>).FirstOrDefault(y => y.id == (int)x[0]) as User);

            var mockApplicationDbContext = new Mock<ApplicationDbContext>();
            mockApplicationDbContext.Setup(x => x.SaveChanges()).Returns(1);
            mockApplicationDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var userController = new UsersController(mockApplicationDbContext.Object);
            return userController;
        }

        private (int, object) CommonCode(IActionResult answer)
        {
            int? statusCode = ((IStatusCodeActionResult)answer).StatusCode;
            //var result = answer;
            var test = answer;
            dynamic result;
            if ((test = answer as ObjectResult) != null)
            {
                result = answer as ObjectResult;
            }
            else if ((test = answer as NotFoundObjectResult) != null)
            {
                result = answer as NotFoundObjectResult;
            }
            else if ((test = answer as BadRequestObjectResult) != null)
            {
                result = answer as BadRequestObjectResult;
            }
            else if ((test = answer as OkObjectResult) != null)
            {
                result = answer as OkObjectResult;
            }
            else if ((test = answer as StatusCodeResult) != null)
            {
                result = answer as StatusCodeResult;
                return (statusCode.Value, null);
            }
            //else if ((test = answer as NotFoundResult) != null)
            //{
            //    result = answer as NotFoundResult;
            //    return (statusCode.Value, null);
            //}
            else
            {
                return (0, null);
            }
            return (statusCode.Value, result.Value);
        }

        private void WriteTestContext(IActionResult answer)
        {
            (var status, var response) = CommonCode(answer);

            TestContext.WriteLine("Status Code: " + status);
            if (response == null)
            {
                TestContext.WriteLine("Content response is null");
                return;
            }
            TestContext.WriteLine(JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.Indented));
        }

        [TestMethod]
        public void TestAddUser()
        {
            // Arrange
            var userController = GetDefaultUserController();

            // Act
            IActionResult answer = userController.Add(
                new Models.AddUserDto
                {
                    name = "Test",
                    email = "Test@test.com",
                    password = "Test123456"
                }
            );
            WriteTestContext(answer);
            (var status, var response) = CommonCode(answer);
            var user = response as User;

            // Assert
            Assert.AreEqual(201, status);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void TestGetAll()
        {
            // Arrange
            var userController = GetDefaultUserController();

            // Act
            //this.TestAddUser();
            IActionResult answer = userController.Get();
            //OkObjectResult? result = answer as OkObjectResult;
            //List<User> value = result.Value as List<User>;
            WriteTestContext(answer);
            (var status, var response) = CommonCode(answer);
            var users = response as List<User>;

            // Assert
            Assert.IsNotNull(users);
        }

        [TestMethod]
        public void TestAddAndGetAll()
        {
            // Arrange
            var userController = GetDefaultUserController();

            // Act
            IActionResult answer = userController.Add(
                new Models.AddUserDto
                {
                    name = "Test",
                    email = "Test@test.com",
                    password = "Test123456"
                }
            );
            WriteTestContext(answer);
            (var status1, var response1) = CommonCode(answer);
            var user = response1 as User;

            // Assert
            Assert.AreEqual(201, status1);
            Assert.IsNotNull(user);

            answer = userController.Get();
            //OkObjectResult? result = answer as OkObjectResult;
            //List<User> value = result.Value as List<User>;
            WriteTestContext(answer);
            (var status2, var response2) = CommonCode(answer);
            var users = response2 as List<User>;

            // Assert
            Assert.IsNotNull(users);
        }

        [TestMethod]
        public void TestAddIncomplete()
        {
            // Arrange
            var userController = GetDefaultUserController();

            // Act
            IActionResult answer = userController.Add(
                new Models.AddUserDto
                {
                    name = "Test",
                    email = "Test",
                    password = ""
                }
            );
            WriteTestContext(answer);
            (var status, var response) = CommonCode(answer);

            // Assert
            Assert.AreNotEqual(201, status);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void TestGet(int Id)
        {
            // Arrange
            var userController = GetDefaultUserController();

            // Act
            IActionResult answer = userController.Get(Id);
            WriteTestContext(answer);
            (var status, var response) = CommonCode(answer);
            User user = response as User;

            // Assert
            Assert.AreEqual(200, status);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void TestGetWrongId()
        {
            // Arrange
            var userController = GetDefaultUserController();

            // Act
            IActionResult answer = userController.Get(0);
            WriteTestContext(answer);
            (var status, var response) = CommonCode(answer);

            // Assert
            Assert.AreNotEqual(200, status);
        }

        [TestMethod]
        public void TestUpdate()
        {
            // Arrange
            var userController = GetDefaultUserController();
            
            // Act
            IActionResult answer = userController.Update(
                1,
                new Models.AddUserDto
                {
                    name = "Test2",
                    email = "Test2@test.com",
                    password = "Test123456"
                }
            );
            WriteTestContext(answer);
            (var status, var response) = CommonCode(answer);

            // Assert
            Assert.AreEqual(200, status);
        }

        [TestMethod]
        public void TestUpdateIncomplete()
        {
            // Arrange
            var userController = GetDefaultUserController();

            // Act
            IActionResult answer = userController.Update(
                1,
                new Models.AddUserDto
                {
                    name = "Test2",
                    email = "Test2",
                    password = ""
                }
            );
            WriteTestContext(answer);
            (var status, var response) = CommonCode(answer);

            // Assert
            Assert.AreNotEqual(200, status);
        }

        [TestMethod]
        public void TestUpdateWrongId()
        {
            // Arrange
            var userController = GetDefaultUserController();

            // Act
            IActionResult answer = userController.Update(
                0,
                new Models.AddUserDto
                {
                    name = "Test2",
                    email = "Test2",
                    password = "Test2"
                }
            );
            WriteTestContext(answer);
            (var status, var response) = CommonCode(answer);

            // Assert
            Assert.AreNotEqual(200, status);
        }

        [TestMethod]
        public void TestDelete()
        {
            // Arrange
            var userController = GetDefaultUserController();

            // Act
            IActionResult answer = userController.Delete(1);
            WriteTestContext(answer);
            (var status, var response) = CommonCode(answer);

            // Assert
            Assert.AreEqual(200, status);
        }

        [TestMethod]
        public void TestDeleteWrongId()
        {
            // Arrange
            var userController = GetDefaultUserController();

            // Act
            IActionResult answer = userController.Delete(0);
            WriteTestContext(answer);
            (var status, var response) = CommonCode(answer);

            // Assert
            Assert.AreNotEqual(200, status);
        }
    }
}
