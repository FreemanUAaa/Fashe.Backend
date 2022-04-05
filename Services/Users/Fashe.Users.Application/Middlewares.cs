using Fashe.Users.Application.Helpers.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Fashe.Users.Application
{
    public static class Middlewares
    {
        public static IApplicationBuilder AddApplicationMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            return app;
        }
    }
}
