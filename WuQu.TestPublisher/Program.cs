using System;

namespace WuQu.TestPublisher
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Numerics;
    using System.Threading.Tasks;

    internal class Program
    {
        private static readonly Random Random = new();
        private static readonly HttpClient Client = new();

        internal static async Task Main(string[] args)
        {
            await PublishLoop();
        }

        private static async Task PublishLoop()
        {
            Console.WriteLine("Beginning publish loop.");

            do
            {
                var delay = TimeSpan.FromSeconds(0.5 + Random.NextDouble() * 2);
                Console.WriteLine($"Delaying publish for {delay.TotalSeconds.ToString("N")} seconds");
                await Task.Delay(delay);

                var payload = new Position
                {
                    X = (float) ((Random.NextDouble() - 0.5d) * 20d),
                    Y = (float) ((Random.NextDouble() - 0.5d) * 20d)
                };

                var response = await Client.PostAsJsonAsync(
                    "http://localhost:5500/api/PublishRequest",
                    new
                    {
                        Type = "Position",
                        Payload = payload
                    });
                response.EnsureSuccessStatusCode();
            } while (true);
        }
    }
}