using IBSYS.PPS.Controllers;
using IBSYS.PPS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace IBSYS.PPS.Controller.Tests
{
    public class BicycleControllerIntegrationTest : ControllerIntegrationTests
    {
        public BicycleControllerIntegrationTest() : base(
            new DbContextOptionsBuilder<IbsysDatabaseContext>()
            .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ibsys;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
            .Options) 
        {

        }

        [Fact]
        public async Task Get_All_Bicycles()
        {
            using (var context = new IbsysDatabaseContext(ContextOptions))
            {
                // Arrange
                var mockLogger = new Mock<ILogger<TestController>>();
                var controller = new TestController(mockLogger.Object, context);

                // Act
                var allBicycles = await controller.GetAllBicycles();
                
                // Assert
                Assert.Equal(3, allBicycles.Count);
            }
        }

        [Fact]
        public async Task Get_One_Bicycle()
        {
            using (var context = new IbsysDatabaseContext(ContextOptions))
            {
                // Arrange
                var mockLogger = new Mock<ILogger<TestController>>();
                var controller = new TestController(mockLogger.Object, context);

                // Act
                var oneBicycle = await controller.GetOneBicycle("P1");

                // Assert
                Assert.Equal("P1", oneBicycle.ProductName);
            }
        }

        [Fact]
        public async Task Get_Labor_And_Machine_Costs()
        {
            using (var context = new IbsysDatabaseContext(ContextOptions))
            {
                // Arrange
                var mockLogger = new Mock<ILogger<TestController>>();
                var controller = new TestController(mockLogger.Object, context);

                // Act
                var laborAndMachineCosts = await controller.GetLaborAndMachineCosts();

                // Assert
                Assert.Equal(14, laborAndMachineCosts.Count);
            }
        }

        [Fact]
        public async Task Get_Self_Production_Items()
        {
            using (var context = new IbsysDatabaseContext(ContextOptions))
            {
                // Arrange
                var mockLogger = new Mock<ILogger<TestController>>();
                var controller = new TestController(mockLogger.Object, context);

                // Act
                var selfProductionItems = await controller.GetSelfProductionItems();

                // Assert
                Assert.Equal(30, selfProductionItems.Count);
            }
        }

        [Fact]
        public async Task Get_All_Purchased_Items()
        {
            using (var context = new IbsysDatabaseContext(ContextOptions))
            {
                // Arrange
                var mockLogger = new Mock<ILogger<TestController>>();
                var controller = new TestController(mockLogger.Object, context);

                // Act
                var purchasedItems = await controller.GetPurchasedItems();

                // Assert
                Assert.Equal(29, purchasedItems.Count);
            }
        }
    }
}
