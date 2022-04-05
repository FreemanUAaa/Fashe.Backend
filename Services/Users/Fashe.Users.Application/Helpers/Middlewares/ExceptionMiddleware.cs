using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Fashe.Users.Application.Helpers.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next) => this.next = next;

        public async Task Invoke(HttpContext http)
        {
            try
            {
                await next.Invoke(http);
            }
            catch (Exception error)
            {
                http.Response.StatusCode = StatusCodes.Status400BadRequest;
                http.Response.ContentType = "application/json; charset=utf-8";
                string errorMessage = $"{{\"error\": \"{error.Message}\" }}";
                await http.Response.WriteAsync(errorMessage);
            }
        }
    }
}
