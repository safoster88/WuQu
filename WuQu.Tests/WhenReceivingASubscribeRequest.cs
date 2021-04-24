namespace WuQu.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FakeItEasy;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using Microsoft.Extensions.DependencyInjection;
    using WuQu.HeartBeat;
    using WuQu.Subscribing;
    using Xunit;

    public class WhenReceivingASubscribeRequest
    {
        private readonly SubscribeRequest request = new SubscribeRequest
        {
            BaseAddress = "http://some-address.com",
            HeartBeatEndPoint = "heartbeat",
            Routes = new []
            {
                new Route
                {
                    Type = "Messages",
                    EndPoint = "api/Messages"
                },
                new Route
                {
                    Type = "UsersChanged",
                    EndPoint = "api/Users"
                }
            }
        };
        
        [Fact]
        public async Task ThenAHeartBeatIsMade()
        {
            var bs = new TestBootstrapper();

            await bs.Mediator.Send(request);

            A.CallTo(() => bs.FakeHttpService.Get("http://some-address.com/heartbeat"))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ThenASubscriptionShouldBeAddedToTheSubscriptions()
        {
            var bs = new TestBootstrapper();

            await bs.Mediator.Send(request);

            bs.Services.GetRequiredService<Subscriptions>().Count.Should().Be(1);
        }

        [Fact]
        public async Task ThenTheCorrectFieldsAreSetOnTheSubscription()
        {
            var bs = new TestBootstrapper();

            await bs.Mediator.Send(request);

            using var _ = new AssertionScope();

            var subscription = bs.Services.GetRequiredService<Subscriptions>().Single();

            subscription.BaseAddress.Should().Be(request.BaseAddress);
            subscription.HeartBeatEndPoint.Should().Be(request.HeartBeatEndPoint);
            subscription.Routes.Length.Should().Be(2);
            subscription.Routes.Should().ContainSingle(x =>
                x.Type == request.Routes[0].Type && x.EndPoint == request.Routes[0].EndPoint);
            subscription.Routes.Should().ContainSingle(x =>
                x.Type == request.Routes[1].Type && x.EndPoint == request.Routes[1].EndPoint);
        }
    }

    public class WhenReceivingASubscribeRequestButTheHeartBeatFails
    {
        [Fact]
        public async Task ThenAHeartBeatFailedExceptionIsThrown()
        {
            var bs = new TestBootstrapper();

            A.CallTo(() => bs.FakeHttpService.Get(A<string>._))
                .Throws<Exception>();

            await FluentActions.Awaiting(() => bs.Mediator.Send(new SubscribeRequest
            {
                BaseAddress = "https://mysafeaddress.com",
                HeartBeatEndPoint = "myHb"
            })).Should().ThrowAsync<HeartBeatFailedException>();
        }
    }
}