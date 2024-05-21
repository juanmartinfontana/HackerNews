namespace HackerNews.Interface
{
    public interface IHttpClientAdapter
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}
