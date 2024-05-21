using HackerNews.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace HackerNews.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IHackerNewsRepository _repository;
    private readonly IMemoryCache _cache;

    public WeatherForecastController(IHackerNewsRepository repository, IMemoryCache cache)
    {
        this._repository = repository;
        this._cache = cache;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<HackerNewsStory>>> Index(string? searchTerm, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            List<HackerNewsStory> stories = new List<HackerNewsStory>();

            if (_cache.TryGetValue("HackerNewsStories", out stories))
            {
                stories = FilterStories(stories, searchTerm);
            }
            else
            {
                var response = await _repository.NewsStoriesAsync();
                if (response.IsSuccessStatusCode)
                {
                    var storiesResponse = await response.Content.ReadAsStringAsync();
                    var newstIds = JsonConvert.DeserializeObject<List<int>>(storiesResponse);

                    var tasks = newstIds.Select(GetStoryAsync);
                    stories = (await Task.WhenAll(tasks)).ToList();

                    _cache.Set("HackerNewsStories", stories, TimeSpan.FromMinutes(5));

                    stories = FilterStories(stories, searchTerm);
                }
            }

            var paginatedStories = stories.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var totalCount = stories.Count;

            return Ok(new PaginatedResult<HackerNewsStory>(paginatedStories, totalCount));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    private async Task<HackerNewsStory> GetStoryAsync(int storyId)
    {
        var cacheKey = $"HackerNewsStory_{storyId}";

        if (_cache.TryGetValue(cacheKey, out HackerNewsStory story))
        {
            return story;
        }
        else
        {
            var response = await _repository.GetStoryByIdAsync(storyId);
            if (response.IsSuccessStatusCode)
            {
                var storyResponse = await response.Content.ReadAsStringAsync();
                story = JsonConvert.DeserializeObject<HackerNewsStory>(storyResponse);

                _cache.Set(cacheKey, story, TimeSpan.FromHours(1));

                return story;
            }
        }
        return null;
    }

    private List<HackerNewsStory> FilterStories(List<HackerNewsStory> stories, string searchTerm)
    {
        if (!String.IsNullOrEmpty(searchTerm))
        {
            var search = searchTerm.ToLower();
            stories = stories.Where(s =>
                s.Title.ToLower().Contains(search) || s.By.ToLower().Contains(search))
                .ToList();
        }
        return stories;
    }
}
