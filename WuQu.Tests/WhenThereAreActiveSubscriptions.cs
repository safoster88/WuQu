namespace WuQu.Tests
{
    using System;
    using System.Threading.Tasks;
    using FakeItEasy;
    using WuQu.Subscribing;
    using Xunit;

    public class WhenThereAreActiveSubscriptions
    {
        private readonly SubscribeRequest[] subscribeRequests = new[]
        {
            new SubscribeRequest
            {
                BaseAddress = "https://myaddress.org",
                HeartBeatEndPoint = "api/heartbeat",
                Routes = new[]
                {
                    new Route
                    {
                        Type = "Position",
                        EndPoint = "api/position"
                    },
                    new Route
                    {
                        Type = "Users",
                        EndPoint = "api/Users"
                    }
                }
            },
            new SubscribeRequest
            {
                BaseAddress = "https://some-url.co.uk",
                HeartBeatEndPoint = "wq/heartbeat",
                Routes = new[]
                {
                    new Route
                    {
                        Type = "Position",
                        EndPoint = "wq/position"
                    },
                    new Route
                    {
                        Type = "LogOn",
                        EndPoint = "wq/log-on"
                    }
                }
            }
        };
        
        private readonly TestBootstrapper bootstrapper = new();
        
        public WhenThereAreActiveSubscriptions()
        {
            // Setup subscriptions
            foreach (var subscribeRequest in subscribeRequests)
            {
                bootstrapper.Mediator.Send(subscribeRequest).Wait();
            }
        }

        [Fact]
        public async Task ThenARepeatingHeartBeatMustOccur()
        {
            void AssertHeartBeatOccured(string endpoint, int numberOfTimes)
            {
                A.CallTo(() => bootstrapper.FakeHttpService.Get(endpoint))
                    .MustHaveHappened(numberOfTimes, Times.Exactly);
            }

            var endpoint1 = "https://myaddress.org/api/heartbeat";
            var endpoint2 = "https://some-url.co.uk/wq/heartbeat";

            var numberOfWaits = 3;

            for (var i = 0; i < numberOfWaits; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(bootstrapper.HeartBeatOptions.HeartBeatIntervalInSeconds));
                
                // first 1 accounts for initial heartbeat on subscription.
                AssertHeartBeatOccured(endpoint1, 1 + i + 1);
                AssertHeartBeatOccured(endpoint2, 1 + i + 1);
            }
        }
    }
}