using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Skola.API.Controllers;
using Skola.API.Models;
using Skola.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Skola.API.Tests.Controllers
{
    public class StudentsControllerTests
    {
        private readonly Mock<IStudentService> _mockStudentService;
        private readonly Mock<ILogger<StudentsController>> _mockLogger;
        private readonly StudentsController _controller;

        public StudentsControllerTests()
        {
            _mockStudentService = new Mock<IStudentService>();
            _mockLogger = new Mock<ILogger<StudentsController>>();
            _controller = new StudentsController(_mockStudentService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetStudentsBySchoolName_ReturnsOkResult_WithPagedResult()
        {
            // Arrange
            var schoolName = "Test School";
            var pageNumber = 1;
            var pageSize = 10;
            var studentResults = new List<StudentResult>
            {
                new StudentResult { StudentID = 1, Name = "John Doe", TotalAbsentDays = 5 }
            };
            var pagedResult = new PagedResult<StudentResult>(studentResults, studentResults.Count, pageNumber, pageSize);

            _mockStudentService
                .Setup(s => s.GetStudentsBySchoolNameAsync(schoolName, pageNumber, pageSize))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetStudentsBySchoolName(schoolName, pageNumber, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResult = Assert.IsType<PagedResult<StudentResult>>(okResult.Value);
            Assert.Equal(pagedResult.Items, actualResult.Items);
            Assert.Equal(pagedResult.TotalCount, actualResult.TotalCount);
            Assert.Equal(pagedResult.PageNumber, actualResult.PageNumber);
            Assert.Equal(pagedResult.PageSize, actualResult.PageSize);

            _mockStudentService.Verify(s => s.GetStudentsBySchoolNameAsync(schoolName, pageNumber, pageSize), Times.Once);

            // Verify logging without using extension methods directly in Moq setup/verification
            _mockLogger.Verify(
                l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"GetStudentsBySchoolName called with parameters schoolName: {schoolName}, pageNumber: {pageNumber}, pageSize: {pageSize}")),
                    null,
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public async Task GetStudentsBySchoolName_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var schoolName = "Test School";
            var pageNumber = 1;
            var pageSize = 10;
            var exception = new Exception("Test exception");

            _mockStudentService
                .Setup(s => s.GetStudentsBySchoolNameAsync(schoolName, pageNumber, pageSize))
                .ThrowsAsync(exception);

            // Act
            var result = await _controller.GetStudentsBySchoolName(schoolName, pageNumber, pageSize);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal($"Internal server error: {exception.Message}", statusCodeResult.Value);

            _mockStudentService.Verify(s => s.GetStudentsBySchoolNameAsync(schoolName, pageNumber, pageSize), Times.Once);

            // Verify logging without using extension methods directly in Moq setup/verification
            _mockLogger.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error occurred in GetStudentsBySchoolName with parameters schoolName: {schoolName}, pageNumber: {pageNumber}, pageSize: {pageSize}")),
                    exception,
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
