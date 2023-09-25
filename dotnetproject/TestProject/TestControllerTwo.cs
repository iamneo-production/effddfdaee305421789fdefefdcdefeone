using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using dotnetmicroservicetwo.Controllers;
using dotnetmicroservicetwo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
namespace dotnetmicroservicetwo.Tests
{
    [TestFixture]
    public class ComplaintControllerTests
    {
        private ComplaintController _ComplaintController;
        private ComplaintDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Initialize an in-memory database for testing
            var options = new DbContextOptionsBuilder<ComplaintDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ComplaintDbContext(options);
            _context.Database.EnsureCreated(); // Create the database

            // Seed the database with sample data
            _context.Complaints.AddRange(new List<Complaint>
            {
                new Complaint { ComplaintID = 1, CustomerID="AT1031", ComplaintType="Mail",ComplaintDate=DateTime.Parse("2023-05-10"), ComplaintStatus="Closed" },
                new Complaint { ComplaintID = 2, CustomerID="AT1032", ComplaintType="SMS",ComplaintDate=DateTime.Parse("2023-07-10"), ComplaintStatus="Open" },
                new Complaint { ComplaintID = 3, CustomerID="AT1033", ComplaintType="Call",ComplaintDate=DateTime.Parse("2023-08-10"), ComplaintStatus="Closed" }
            });
            _context.SaveChanges();

            _ComplaintController = new ComplaintController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Delete the in-memory database after each test
            _context.Dispose();
        }
        [Test]
        public void ComplaintClassExists()
        {
            // Arrange
            Type ComplaintType = typeof(Complaint);

            // Act & Assert
            Assert.IsNotNull(ComplaintType, "Complaint class not found.");
        }
        [Test]
        public void Complaint_Properties_ComplaintType_ReturnExpectedDataTypes()
        {
            // Arrange
            Complaint complaint = new Complaint();
            PropertyInfo propertyInfo = complaint.GetType().GetProperty("ComplaintType");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "ComplaintType property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "ComplaintType property type is not string.");
        }
[Test]
        public void Complaint_Properties_ComplaintStatus_ReturnExpectedDataTypes()
        {
            // Arrange
            Complaint complaint = new Complaint();
            PropertyInfo propertyInfo = complaint.GetType().GetProperty("ComplaintStatus");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "ComplaintStatus property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "ComplaintStatus property type is not string.");
        }

        [Test]
        public async Task GetAllComplaints_ReturnsOkResult()
        {
            // Act
            var result = await _ComplaintController.GetAllComplaints();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllComplaints_ReturnsAllComplaints()
        {
            // Act
            var result = await _ComplaintController.GetAllComplaints();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<IEnumerable<Complaint>>(okResult.Value);
            var complaints = okResult.Value as IEnumerable<Complaint>;

            var ComplaintCount = complaints.Count();
            Assert.AreEqual(3, ComplaintCount); // Assuming you have 3 Complaints in the seeded data
        }


        [Test]
        public async Task AddComplaint_ValidData_ReturnsOkResult()
        {
            // Arrange
            var newComplaint = new Complaint
            {
 CustomerID="AT1034", ComplaintType="Mail",ComplaintDate=DateTime.Parse("2023-02-10"), ComplaintStatus="Closed"
            };

            // Act
            var result = await _ComplaintController.AddComplaint(newComplaint);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteComplaint_ValidId_ReturnsNoContent()
        {
            // Arrange
              // var controller = new ComplaintsController(context);

                // Act
                var result = await _ComplaintController.DeleteComplaint(1) as NoContentResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task DeleteComplaint_InvalidId_ReturnsBadRequest()
        {
                   // Act
                var result = await _ComplaintController.DeleteComplaint(0) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("Not a valid Complaint id", result.Value);
        }
    }
}
