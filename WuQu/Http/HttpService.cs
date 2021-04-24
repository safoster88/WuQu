namespace WuQu.Http
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task Get(string address)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetAsync(address);
            response.EnsureSuccessStatusCode();
        }

        public async Task Post(string address, object payload)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(address, payload);
            response.EnsureSuccessStatusCode();
        }
    }
}