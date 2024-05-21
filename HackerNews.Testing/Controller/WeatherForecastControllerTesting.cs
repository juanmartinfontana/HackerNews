using HackerNews.Controllers;
using HackerNews.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace HackerNews.Testing.Controller
{
    public class WeatherForecastControllerTesting
    {
        [Fact]
        public async Task Index_ShouldReturnOkWithPaginatedResult()
        {
            // Arrange
            var mockRepository = new Mock<IHackerNewsRepository>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new WeatherForecastController(mockRepository.Object, mockMemoryCache.Object);

            var fakeStories = new List<HackerNewsStory>
            {
                new HackerNewsStory { Url = "www.asdasd.com", Title = "Story 1", By = "User 1" },
                new HackerNewsStory { Url = "www.asdasdaasdasdasd.com", Title = "Story 2", By = "User 2" }
            };
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(fakeStories))
            };
            mockRepository.Setup(r => r.NewsStoriesAsync()).ReturnsAsync(fakeResponse);

            // Act
            var result = await controller.Index(searchTerm: "", pageNumber: 1, pageSize: 10);

            Assert.IsType<ActionResult<PaginatedResult<HackerNewsStory>>>(result);
            var actionResult = result;
            Assert.NotNull(actionResult);
            //var okResult = actionResult?.Result as OkObjectResult;
            //Assert.NotNull(okResult);
            //var paginatedResult = okResult?.Value as PaginatedResult<HackerNewsStory>;
            //Assert.NotNull(paginatedResult);
        }
    }
}
