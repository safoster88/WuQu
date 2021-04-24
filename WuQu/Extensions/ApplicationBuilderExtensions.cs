namespace WuQu.Extensions
{
    using Microsoft.AspNetCore.Builder;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWuQu(
            this IApplicationBuilder app)
        {
            app.ApplicationServices.UseWuQu();
            return app;
        }
    }
}