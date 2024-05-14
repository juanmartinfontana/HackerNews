using HackerNews.Interface;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNewsApp.Repository
{
    public class HackerNewsRepository : IHackerNewsRepository
    {
        private static HttpClient hackerNewsClient = new HttpClient();

        public async Task<HttpResponseMessage> NewsStoriesAsync()
        {
            return await hackerNewsClient.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
        }

        public async Task<HttpResponseMessage> GetStoryByIdAsync(int id)
        {
            return await hackerNewsClient.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", id));
        }
    }
}
