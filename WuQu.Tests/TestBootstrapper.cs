namespace WuQu.Tests
{
    using System;
    using FakeItEasy;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using WuQu.Extensions;
    using WuQu.HeartBeat;
    using WuQu.Http;

    public class TestBootstrapper
    {
        public TestBootstrapper()
        {
            var services = new ServiceCollection();
            services.AddWuQu();

            services.AddSingleton(FakeHttpService);
            services.AddSingleton(HeartBeatOptions);
            
            Services = services.BuildServiceProvider();

            Services.UseWuQu();
        }

        public IServiceProvider Services { get; }

        public IMediator Mediator => Services.GetRequiredService<IMediator>();

        public IHttpService FakeHttpService { get; } = A.Fake<IHttpService>();

        public HeartBeatOptions HeartBeatOptions { get; } = new HeartBeatOptions
        {
            HeartBeatIntervalInSeconds = 1
        };
    }
}