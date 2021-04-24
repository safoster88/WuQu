namespace WuQu.TestSubscriber
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public class WuQuSubscriber : IHostedService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger logger;

        public WuQuSubscriber(
            IHttpClientFactory httpClientFactory,
            ILogger logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("WuQu");

            var response = await client.PostAsJsonAsync("api/SubscribeRequest", new
            {
                BaseAddress = "http://localhost:5000",
                HeartBeatEndPoint = "wuqu/HeartBeat",
                Routes = new[]
                {
                    new
                    {
                        Type = "Position",
                        EndPoint = "wuqu/Positions"
                    }
                }
            });

            response.EnsureSuccessStatusCode();
            
            logger.Information("Subscribed to positions");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}