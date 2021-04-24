namespace WuQu.Tests
{
    using System;
    using FakeItEasy;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using WuQu.Extensions;
    using WuQu.Http;

    public class TestBootstrapper
    {
        public TestBootstrapper()
        {
            var services = new ServiceCollection();
            services.AddWuQu();

            services.AddSingleton(FakeHttpService);
            
            Services = services.BuildServiceProvider();
        }

        public IServiceProvider Services { get; }

        public IMediator Mediator => Services.GetRequiredService<IMediator>();

        public IHttpService FakeHttpService { get; } = A.Fake<IHttpService>();
    }
}