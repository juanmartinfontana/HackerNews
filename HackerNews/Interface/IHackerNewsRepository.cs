namespace HackerNews.Interface
{
    public interface IHackerNewsRepository
    {
        Task<HttpResponseMessage> NewsStoriesAsync();
        Task<HttpResponseMessage> GetStoryByIdAsync(int id);
    }
}
