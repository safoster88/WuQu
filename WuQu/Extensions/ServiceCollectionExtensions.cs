namespace WuQu.Extensions
{
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using WuQu.HeartBeat;
    using WuQu.Http;
    using WuQu.Subscribing;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWuQu(
            this IServiceCollection services)
        {
            services.AddSingleton(Log.Logger);
            services.AddMediatR(typeof(Startup));
            services.AddSingleton<Subscriptions>();
            services.AddSingleton<HeartBeater>();
            services.AddSingleton<HeartBeatService>();
            services.AddHttpClient();
            return services;
        }

        public static IServiceCollection AddWuQuDal(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<HeartBeatOptions>(configuration.GetSection(HeartBeatOptions.ConfigurationKey));
            services.AddSingleton<IHttpService, HttpService>();
            return services;
        }
    }
}