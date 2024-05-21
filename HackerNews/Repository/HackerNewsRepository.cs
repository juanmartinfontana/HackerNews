using HackerNews.Interface;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNewsApp.Repository
{
    public class HackerNewsRepository : IHackerNewsRepository
    {
        private readonly IHttpClientAdapter _httpClientAdapter;

        public HackerNewsRepository(IHttpClientAdapter httpClientAdapter)
        {
            _httpClientAdapter = httpClientAdapter;
        }

        public async Task<HttpResponseMessage> NewsStoriesAsync()
        {
            return await _httpClientAdapter.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
        }

        public async Task<HttpResponseMessage> GetStoryByIdAsync(int id)
        {
            return await _httpClientAdapter.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", id));
        }
    }
}
