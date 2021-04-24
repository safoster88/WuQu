namespace WuQu.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using WuQu.HeartBeat;

    public static class ServiceProviderExtensions
    {
        public static IServiceProvider UseWuQu(
            this IServiceProvider services)
        {
            _ = services.GetRequiredService<HeartBeatService>().Start(default);
            return services;
        }
    }
}