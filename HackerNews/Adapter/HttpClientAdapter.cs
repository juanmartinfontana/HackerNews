using HackerNews.Interface;

namespace HackerNews.Adapter
{
    public class HttpClientAdapter : IHttpClientAdapter
    {
        private readonly HttpClient _httpClient;

        public HttpClientAdapter(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return _httpClient.GetAsync(requestUri);
        }
    }
}
