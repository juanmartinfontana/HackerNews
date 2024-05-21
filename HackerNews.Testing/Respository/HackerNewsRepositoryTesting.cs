using HackerNewsApp.Repository;
using HackerNews.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Testing.Respository
{
    public class HackerNewsRepositoryTesting
    {
        [Fact]
        public async Task NewsStoriesAsync_ShouldReturnHttpResponseMessage()
        {
            // Arrange
            var expectedStatusCode = HttpStatusCode.OK;
            var expectedResponse = new HttpResponseMessage(expectedStatusCode);
            var mockHttpClientAdapter = new Mock<IHttpClientAdapter>();
            mockHttpClientAdapter.Setup(c => c.GetAsync(It.IsAny<string>())).ReturnsAsync(expectedResponse);

            var hackerNewsRepository = new HackerNewsRepository(mockHttpClientAdapter.Object);

            // Act
            var response = await hackerNewsRepository.NewsStoriesAsync();

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task GetStoryByIdAsync_ShouldReturnHttpResponseMessage()
        {
            // Arrange
            var id = 123;
            var expectedStatusCode = HttpStatusCode.OK;
            var expectedResponse = new HttpResponseMessage(expectedStatusCode);
            var mockHttpClientAdapter = new Mock<IHttpClientAdapter>();
            mockHttpClientAdapter.Setup(c => c.GetAsync(It.IsAny<string>())).ReturnsAsync(expectedResponse);
            var hackerNewsRepository = new HackerNewsRepository(mockHttpClientAdapter.Object);

            // Act
            var response = await hackerNewsRepository.GetStoryByIdAsync(id);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}
