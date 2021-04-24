namespace WuQu.Tests
{
    using System.Numerics;
    using System.Threading.Tasks;
    using FakeItEasy;
    using WuQu.Publishing;
    using WuQu.Subscribing;
    using Xunit;

    public class WhenReceivingAPublishRequest
    {
        [Fact]
        public async Task ThenAPostShouldBeMadeToAnySubscriptionRoutesMatchingTheType()
        {
            var bs = new TestBootstrapper();

            await bs.Mediator.Send(new SubscribeRequest
            {
                BaseAddress = "https://root-address.co.uk",
                HeartBeatEndPoint = "hb",
                Routes = new[]
                {
                    new Route
                    {
                        Type = "Position",
                        EndPoint = "api/Positions"
                    },
                    new Route
                    {
                        Type = "User",
                        EndPoint = "api/Users"
                    }
                }
            });
            
            await bs.Mediator.Send(new SubscribeRequest
            {
                BaseAddress = "http://some-other-address.com",
                HeartBeatEndPoint = "heartbeat",
                Routes = new[]
                {
                    new Route
                    {
                        Type = "Position",
                        EndPoint = "api/Positions"
                    }
                }
            });

            var payload = new Vector2(5.2f, 3.4f); 

            await bs.Mediator.Send(new PublishRequest
            {
                Type = "Position",
                Payload = payload
            });

            A.CallTo(() => bs.FakeHttpService.Post("https://root-address.co.uk/api/Positions", payload))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => bs.FakeHttpService.Post("http://some-other-address.com/api/Positions", payload))
                .MustHaveHappenedOnceExactly();
        }
    }
}