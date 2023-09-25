using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using dotnetmicroserviceone.Controllers;
using dotnetmicroserviceone.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
namespace dotnetmicroserviceone.Tests
{
    [TestFixture]
    public class CallControllerTests
    {
        private CallController _CallController;
        private CallDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Initialize an in-memory database for testing
            var options = new DbContextOptionsBuilder<CallDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new CallDbContext(options);
            _context.Database.EnsureCreated(); // Create the database

            // Seed the database with sample data
            _context.Calls.AddRange(new List<Call>
            {
                new Call { CallID = 1, ExecutiveID="EID101",CustomerID="AT1031",CallType="Service",CallDate=DateTime.Parse("2023-06-22"),CallStatus="Completed" },
                new Call { CallID = 2, ExecutiveID="EID102",CustomerID="AT1032",CallType="Complaint",CallDate=DateTime.Parse("2023-05-22"),CallStatus="OnHold" },
                new Call { CallID = 3, ExecutiveID="EID103",CustomerID="AT1033",CallType="New Request",CallDate=DateTime.Parse("2023-04-22"),CallStatus="Transferred" },
            });
            _context.SaveChanges();

            _CallController = new CallController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Delete the in-memory database after each test
            _context.Dispose();
        }
        [Test]
        public void CallClassExists()
        {
            // Arrange
            Type CallType = typeof(Call);

            // Act & Assert
            Assert.IsNotNull(CallType, "Call class not found.");
        }
        [Test]
        public void Call_Properties_CustomerID_ReturnExpectedDataTypes()
        {
            // Arrange
            Call call = new Call();
            PropertyInfo propertyInfo = call.GetType().GetProperty("CustomerID");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "CustomerID property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "CustomerID property type is not string.");
        }
[Test]
        public void Call_Properties_CallType_ReturnExpectedDataTypes()
        {
            // Arrange
            Call call = new Call();
            PropertyInfo propertyInfo = call.GetType().GetProperty("CallType");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "CallType property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "CallType property type is not string.");
        }
        [Test]
        public void Call_Properties_CallStatus_ReturnExpectedDataTypes()
        {
            // Arrange
            Call call = new Call();
            PropertyInfo propertyInfo = call.GetType().GetProperty("CallStatus");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "CallStatus property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "CallStatus property type is not string.");
        }

        [Test]
        public async Task GetAllCalls_ReturnsOkResult()
        {
            // Act
            var result = await _CallController.GetAllCalls();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllCalls_ReturnsAllCalls()
        {
            // Act
            var result = await _CallController.GetAllCalls();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<IEnumerable<Call>>(okResult.Value);
            var calls = okResult.Value as IEnumerable<Call>;

            var CallCount = calls.Count();
            Assert.AreEqual(3, CallCount); // Assuming you have 3 Calls in the seeded data
        }

        [Test]
        public async Task GetCallById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var existingId = 1;

            // Act
            var result = await _CallController.GetCallById(existingId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetCallById_ExistingId_ReturnsCall()
        {
            // Arrange
            var existingId = 1;

            // Act
            var result = await _CallController.GetCallById(existingId);

            // Assert
            Assert.IsNotNull(result);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            var call = okResult.Value as Call;
            Assert.IsNotNull(call);
            Assert.AreEqual(existingId, call.CallID);
        }

        [Test]
        public async Task GetCallById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = 99; // Assuming this ID does not exist in the seeded data

            // Act
            var result = await _CallController.GetCallById(nonExistingId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task AddCall_ValidData_ReturnsOkResult()
        {
            // Arrange
            var newCall = new Call
            {
ExecutiveID="EID104",CustomerID="AT1034",CallType="Service",CallDate=DateTime.Parse("2023-06-22"),CallStatus="Completed"
            };

            // Act
            var result = await _CallController.AddCall(newCall);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteCall_ValidId_ReturnsNoContent()
        {
            // Arrange
              // var controller = new CallsController(context);

                // Act
                var result = await _CallController.DeleteCall(1) as NoContentResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task DeleteCall_InvalidId_ReturnsBadRequest()
        {
                   // Act
                var result = await _CallController.DeleteCall(0) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("Not a valid Call id", result.Value);
        }
    }
}
