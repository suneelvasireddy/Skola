using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Skola.API.Controllers;
using Skola.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Skola.API.Tests.Controllers
{
    public class SchoolsControllerTests2
    {
        private readonly Mock<ISchoolService> _mockSchoolService;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly Mock<ILogger<SchoolsController>> _mockLogger;
        private readonly SchoolsController _controller;

        public SchoolsControllerTests2()
        {
            _mockSchoolService = new Mock<ISchoolService>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockLogger = new Mock<ILogger<SchoolsController>>();
            _controller = new SchoolsController(_mockSchoolService.Object, _mockMemoryCache.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetSchools_ReturnsOkResult_WithSchoolNames_FromCache()
        {
            // Arrange
            var expectedSchoolNames = new List<string> { "School A", "School B" };
            object cacheEntry = expectedSchoolNames;

            _mockMemoryCache
                .Setup(mc => mc.TryGetValue(It.IsAny<string>(), out cacheEntry))
                .Returns(true);

            // Act
            var result = await _controller.GetSchools();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualSchoolNames = Assert.IsType<List<string>>(okResult.Value);
            Assert.Equal(expectedSchoolNames, actualSchoolNames);

            _mockMemoryCache.Verify(mc => mc.TryGetValue(It.IsAny<string>(), out cacheEntry), Times.Once);
            _mockSchoolService.Verify(ss => ss.GetSchoolNamesAsync(), Times.Never);
        }

        [Fact]
        public async Task GetSchools_ReturnsOkResult_WithSchoolNames_FromService()
        {
            // Arrange
            var expectedSchoolNames = new List<string> { "School A", "School B" };
            object cacheEntry = null;

            _mockMemoryCache
                .Setup(mc => mc.TryGetValue(It.IsAny<string>(), out cacheEntry))
                .Returns(false);

            _mockMemoryCache
                .Setup(mc => mc.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            _mockSchoolService
                .Setup(ss => ss.GetSchoolNamesAsync())
                .ReturnsAsync(expectedSchoolNames);

            // Act
            var result = await _controller.GetSchools();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualSchoolNames = Assert.IsType<List<string>>(okResult.Value);
            Assert.Equal(expectedSchoolNames, actualSchoolNames);

            _mockMemoryCache.Verify(mc => mc.TryGetValue(It.IsAny<string>(), out cacheEntry), Times.Once);
            _mockSchoolService.Verify(ss => ss.GetSchoolNamesAsync(), Times.Once);
            _mockMemoryCache.Verify(mc => mc.CreateEntry(It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public async Task GetSchools_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            object cacheEntry = null;
            var exception = new Exception("Test exception");

            _mockMemoryCache
                .Setup(mc => mc.TryGetValue(It.IsAny<string>(), out cacheEntry))
                .Throws(exception);

            // Act
            var result = await _controller.GetSchools();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error", statusCodeResult.Value);

            _mockMemoryCache.Verify(mc => mc.TryGetValue(It.IsAny<string>(), out cacheEntry), Times.Once);

            // Verify the logger was called with the correct parameters
            _mockLogger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while retrieving school names.")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}
