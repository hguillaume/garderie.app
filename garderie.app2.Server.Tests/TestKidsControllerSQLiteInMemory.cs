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
public class TestKidsControllerSQLiteInMemory
{
    public TestContext TestContext { get; set; }

    private KidsController GetDefaultKidsControllerSQLiteInMemory()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        SQLitePCL.Batteries.Init();
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        UsersController userController = new UsersController(context);
        userController.Add(new AddUserDto
        {
            name = "TestUser",
            email = "test@test.com",
            password = "TestPassword"
        });

        DaycaresController daycareController = new DaycaresController(context);
        daycareController.Add(new AddDaycareDto
        {
            name = "TestUser",
            userId = 1
        });

        KidsController controller = new KidsController(context);

        controller.Add(new AddKidDto
        {
            name = "John1",
            daycareId = 1
        });
        controller.Add(new AddKidDto
        {
            name = "John2",
            daycareId = 1
        });
        controller.Add(new AddKidDto
        {
            name = "John3",
            daycareId = 1
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
        //TestContext.WriteLine(JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.Indented));
        TestContext.WriteLine(JsonConvert.SerializeObject(response, new JsonSerializerSettings
        {
            //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        }));
    }

    [TestMethod]
    public void TestGetAll()
    {
        // Arrange
        KidsController controller = GetDefaultKidsControllerSQLiteInMemory();

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
        KidsController controller = GetDefaultKidsControllerSQLiteInMemory();

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
        KidsController controller = GetDefaultKidsControllerSQLiteInMemory();

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
        KidsController controller = GetDefaultKidsControllerSQLiteInMemory();

        // Act
        var answer = controller.Add(new AddKidDto
        {
            name = "John3",
            daycareId = 1
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
        KidsController controller = GetDefaultKidsControllerSQLiteInMemory();

        // Act
        var answer = controller.Add(new AddKidDto
        {
            name = "",
            daycareId = 1
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
        KidsController controller = GetDefaultKidsControllerSQLiteInMemory();

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
        KidsController controller = GetDefaultKidsControllerSQLiteInMemory();
        
        // Act
        var answer = controller.Update(Id, new AddKidDto
        {
            name = "JohnUpdated" + Id,
            daycareId = 1
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
        KidsController controller = GetDefaultKidsControllerSQLiteInMemory();
        
        // Act
        var answer = controller.Update(Id, new AddKidDto
        {
            name = "JohnUpdated" + Id,
            daycareId = 1
        }
        );
        WriteTestContext(answer);
        (var status, var response) = CommonCode(answer);
        
        // Assert
        Assert.AreEqual(404, status);
        Assert.IsNull(response);
    }
}