namespace WuQu.Extensions
{
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
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
            services.AddHttpClient();
            return services;
        }

        public static IServiceCollection AddWuQuDal(
            this IServiceCollection services)
        {
            services.AddSingleton<IHttpService, HttpService>();
            return services;
        }
    }
}