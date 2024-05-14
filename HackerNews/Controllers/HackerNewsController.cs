using HackerNews;
using HackerNews.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace HackerNewsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsRepository _repository;
        private readonly IMemoryCache _cache;

        public HackerNewsController(IHttpClientFactory httpClientFactory, IHackerNewsRepository repository, IMemoryCache cache)
        {
            this._repository = repository;
            this._cache = cache;
        }

        [HttpGet]
        public async Task<List<HackerNewsStory>> Index(string? searchTerm)
        {
            List<HackerNewsStory> stories = new List<HackerNewsStory>();

            // Intenta obtener las historias de la caché
            if (_cache.TryGetValue("HackerNewsStories", out stories))
            {
                // Si las historias están en caché, devuélvelas y filtra según el término de búsqueda
                return FilterStories(stories, searchTerm);
            }
            else
            {
                // Si las historias no están en caché, obténlas de la API de Hacker News
                var response = await _repository.NewsStoriesAsync();
                if (response.IsSuccessStatusCode)
                {
                    var storiesResponse = await response.Content.ReadAsStringAsync();
                    var newstIds = JsonConvert.DeserializeObject<List<int>>(storiesResponse);

                    var tasks = newstIds.Select(GetStoryAsync);
                    stories = (await Task.WhenAll(tasks)).ToList();

                    // Almacena las historias en caché durante un tiempo limitado (por ejemplo, 5 minutos)
                    _cache.Set("HackerNewsStories", stories, TimeSpan.FromMinutes(5));

                    // Filtra las historias según el término de búsqueda
                    return FilterStories(stories, searchTerm);
                }
            }
            return stories;
        }

        private async Task<HackerNewsStory> GetStoryAsync(int storyId)
        {
            var cacheKey = $"HackerNewsStory_{storyId}";

            // Intenta obtener la historia desde la caché
            if (_cache.TryGetValue(cacheKey, out HackerNewsStory story))
            {
                // Si la historia está en caché, devuélvela
                return story;
            }
            else
            {
                // Si la historia no está en caché, obténla de la API de Hacker News
                var response = await _repository.GetStoryByIdAsync(storyId);
                if (response.IsSuccessStatusCode)
                {
                    var storyResponse = await response.Content.ReadAsStringAsync();
                    story = JsonConvert.DeserializeObject<HackerNewsStory>(storyResponse);

                    // Almacena la historia en caché durante un tiempo limitado (por ejemplo, 1 hora)
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
}
