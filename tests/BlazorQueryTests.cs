using System.Net;
using System.Text.Json;
using Moq;
using Moq.Protected;
using Xunit;

namespace BlazorLoader.Tests
{
    public class BlazorQueryTests
    {
        private readonly Mock<IBlazorQueryOption> _mockOptions;
        private readonly string _testUrl = "https://api.example.com/data";
        private readonly string _testKey = "test-key";

        public BlazorQueryTests()
        {
            _mockOptions = new Mock<IBlazorQueryOption>();
        }

        [Fact]
        public async Task ExecuteAsync_SuccessfulRequest_ReturnsExpectedResponse()
        {
            // Arrange
            var expectedData = new TestData { Id = 1, Name = "Test" };
            var jsonResponse = JsonSerializer.Serialize(expectedData);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            var query = new BlazorQuery(_testUrl, _testKey, _mockOptions.Object, client);

            // Act
            var result = await query.ExecuteAsync<TestData>();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasError);
            Assert.Equal(RequestState.Done, result.State);
            Assert.NotNull(result.Response);
            Assert.Equal(expectedData.Id, result.Response.Id);
            Assert.Equal(expectedData.Name, result.Response.Name);
        }

        [Fact]
        public async Task ExecuteAsync_FailedRequest_ReturnsErrorResponse()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            var client = new HttpClient(mockHttpMessageHandler.Object);
            var query = new BlazorQuery(_testUrl, _testKey, _mockOptions.Object, client);

            // Act
            var result = await query.ExecuteAsync<TestData>();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasError);
            Assert.Equal(RequestState.Done, result.State);
            Assert.NotNull(result.Error);
            Assert.IsType<HttpRequestException>(result.Error);
        }

        private class TestData
        {
            public int Id { get; set; }
            public required string Name { get; set; }
        }
    }
}
