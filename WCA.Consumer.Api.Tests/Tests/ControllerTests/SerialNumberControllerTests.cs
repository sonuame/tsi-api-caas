using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WCA.Consumer.Api.Models;
using WCA.Consumer.Api.Services.Contracts;
using WCA.Consumer.Api.Controllers;
using Xunit;

namespace WCA.Customer.Api.Tests
{
    public class SerialNumberControllerTests
    {
        [Fact]
        public async Task GetSerialNumbers_Value_Success()
        {
            var serviceMock = new Mock<ISerialNumberService>(MockBehavior.Strict);
            var serialNumber = TestDataHelper.CreateSerialNumberModel();
            serviceMock.Setup(m => m.GetSerialNumberByValue(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(serialNumber);

            var controller = new SerialNumberController(serviceMock.Object);
            var result = await controller.GetSerialNumbers("fake.user.email@example.com", serialNumber.Value, null);

            Assert.Equal(typeof(OkObjectResult), result.GetType());
            Assert.Equal((int)HttpStatusCode.OK, (result as OkObjectResult).StatusCode);
            var expectedSerialNumber = ((result as OkObjectResult).Value as SerialNumberModel);
            Assert.Equal(expectedSerialNumber.Value, serialNumber.Value);
        }

        [Fact]
        public async Task GetSerialNumbers_Value_NonExistent()
        {
            var serviceMock = new Mock<ISerialNumberService>(MockBehavior.Strict);
            SerialNumberModel serialNumber = null;
            serviceMock.Setup(m => m.GetSerialNumberByValue(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(serialNumber);

            var controller = new SerialNumberController(serviceMock.Object);
            var result = await controller.GetSerialNumbers("fake.user.email@example.com", "non-existent", null);

            Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
            Assert.Equal((int)HttpStatusCode.NotFound, (result as NotFoundObjectResult).StatusCode);
        }

        [Fact]
        public async Task GetSerialNumbers_Filter_SingleMatch()
        {
            var serviceMock = new Mock<ISerialNumberService>(MockBehavior.Strict);
            var serialNumbers = TestDataHelper.CreateSerialNumberModels(1);
            var serialNumber = serialNumbers.First();
            serviceMock.Setup(m => m.GetSerialNumbersByFilter(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<uint?>()))
                .ReturnsAsync(serialNumbers);

            var controller = new SerialNumberController(serviceMock.Object);
            var result = await controller.GetSerialNumbers("fake.user.email@example.com", null, serialNumber.Value);

            Assert.Equal(typeof(OkObjectResult), result.GetType());
            Assert.Equal((int)HttpStatusCode.OK, (result as OkObjectResult).StatusCode);
            var expectedSerialNumbers = (result as OkObjectResult).Value as List<SerialNumberModel>;
            Assert.Single(expectedSerialNumbers);
            Assert.Equal(expectedSerialNumbers[0].Value, serialNumber.Value);
        }

        [Fact]
        public async Task GetSerialNumbers_Filter_MultipleMatches()
        {
            var serviceMock = new Mock<ISerialNumberService>(MockBehavior.Strict);
            var serialNumbers = TestDataHelper.CreateSerialNumberModels(1);
            var serialNumber = serialNumbers.First();
            serviceMock.Setup(m => m.GetSerialNumbersByFilter(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<uint?>()))
                .ReturnsAsync(serialNumbers);

            var controller = new SerialNumberController(serviceMock.Object);
            var result = await controller.GetSerialNumbers("fake.user.email@example.com", null, serialNumber.Value.Substring(0, 10));

            Assert.Equal(typeof(OkObjectResult), result.GetType());
            Assert.Equal((int)HttpStatusCode.OK, (result as OkObjectResult).StatusCode);
            var expectedSerialNumbers = (result as OkObjectResult).Value as List<SerialNumberModel>;
            Assert.Single(expectedSerialNumbers);
            for (int i = 0; i < serialNumbers.Count; i++)
            {
                Assert.Equal(expectedSerialNumbers[i].Value, serialNumbers[i].Value);
            }
        }

        [Fact]
        public async Task GetSerialNumbers_Filter_NonExistent()
        {
            var serviceMock = new Mock<ISerialNumberService>(MockBehavior.Strict);
            serviceMock.Setup(m => m.GetSerialNumbersByFilter(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<uint?>()))
                .ReturnsAsync(new List<SerialNumberModel>());

            var controller = new SerialNumberController(serviceMock.Object);
            var result = await controller.GetSerialNumbers("fake.user.email@example.com", null, "non-existent");

            Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
        }
    }
}
