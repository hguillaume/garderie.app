// Install nuget package Microsoft.EntityFrameworkCore.Sqlite (not only Microsoft.EntityFrameworkCore.Sqlite.Core alone, because it does not come with other dependancies)

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using garderie.app2.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using garderie.app2.Server.Controllers;
using garderie.app2.Server.Models;

namespace garderie.app2.Server.Tests;

[TestClass]
public class TestUsersControllerSQLiteInMemory
{
    public TestContext TestContext { get; set; }

    private UsersController GetDefaultUsersControllerSQLiteInMemory()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        SQLitePCL.Batteries.Init();
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        UsersController controller = new UsersController(context);

        controller.Add(new AddUserDto
        {
            name = "John1",
            email = "test@test.com",
            password = "123456"
        });
        controller.Add(new AddUserDto
        {
            name = "John2",
            email = "test@test.com",
            password = "123456"
        });
        controller.Add(new AddUserDto
        {
            name = "John3",
            email = "test@test.com",
            password = "123456"
        });

        return controller;
    }
    private (int, object) CommonCode(IActionResult answer)
    {
        int? statusCode = ((IStatusCodeActionResult)answer).StatusCode;
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
    public void TestGetAll()
    {
        // Arrange
        UsersController controller = GetDefaultUsersControllerSQLiteInMemory();

        // Act
        var answer = controller.Get();
        WriteTestContext(answer);
        (var status, var response) = CommonCode(answer);

        // Assert
        Assert.AreEqual(200, status);
        Assert.IsNotNull(response);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(2)]
    public void TestGet(int Id)
    {
        // Arrange
        UsersController controller = GetDefaultUsersControllerSQLiteInMemory();

        // Act
        var answer = controller.Get(Id);
        WriteTestContext(answer);
        (var status, var response) = CommonCode(answer);

        // Assert
        Assert.AreEqual(200, status);
        Assert.IsNotNull(response);
    }

    [TestMethod]
    [DataRow(0)]
    public void TestGetNotFound(int Id)
    {
        // Arrange
        UsersController controller = GetDefaultUsersControllerSQLiteInMemory();

        // Act
        var answer = controller.Get(Id);
        WriteTestContext(answer);
        (var status, var response) = CommonCode(answer);

        // Assert
        Assert.AreEqual(404, status);
        Assert.IsNull(response);
    }

    [TestMethod]
    public void TestAdd()
    {
        // Arrange
        UsersController controller = GetDefaultUsersControllerSQLiteInMemory();

        // Act
        var answer = controller.Add(new AddUserDto
        {
            name = "John3",
            email = "test@tes.com",
            password = "123456"
        });
        WriteTestContext(answer);
        (var status, var response) = CommonCode(answer);

        // Assert
        Assert.AreEqual(201, status);
        Assert.IsNotNull(response);
    }

    [TestMethod]
    public void TestAddBadRequest()
    {
        // Arrange
        UsersController controller = GetDefaultUsersControllerSQLiteInMemory();

        // Act
        var answer = controller.Add(new AddUserDto
        {
            name = "John3",
            email = "test",
            password = "123456"
        });
        WriteTestContext(answer);
        (var status, var response) = CommonCode(answer);

        // Assert
        Assert.AreEqual(400, status);
        Assert.IsNotNull(response);
    }

    [TestMethod]
    [DataRow(3)]
    public void TestRemove(int Id)
    {
        // Arrange
        UsersController controller = GetDefaultUsersControllerSQLiteInMemory();

        // Act
        var answer = controller.Delete(Id);
        WriteTestContext(answer);
        (var status, var response) = CommonCode(answer);

        // Assert
        Assert.AreEqual(200, status);
        Assert.IsNull(response);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(2)]
    public void TestUpdate(int Id) {
        
        // Arrange
        UsersController controller = GetDefaultUsersControllerSQLiteInMemory();
        
        // Act
        var answer = controller.Update(Id, new AddUserDto
        {
            name = "JohnUpdated" + Id,
            email = "test@test.com",
            password = "123456"
        }
        );
        WriteTestContext(answer);
        (var status, var response) = CommonCode(answer);
        
        // Assert
        Assert.AreEqual(200, status);
        Assert.IsNotNull(response);
    }

    [TestMethod]
    [DataRow(0)]
    public void TestUpdateNotFound(int Id)
    {
        // Arrange
        UsersController controller = GetDefaultUsersControllerSQLiteInMemory();
        
        // Act
        var answer = controller.Update(Id, new AddUserDto
        {
            name = "JohnUpdated" + Id,
            email = "abc@abc.com",
            password = "123456"
        }
        );
        WriteTestContext(answer);
        (var status, var response) = CommonCode(answer);
        
        // Assert
        Assert.AreEqual(404, status);
        Assert.IsNull(response);
    }
}