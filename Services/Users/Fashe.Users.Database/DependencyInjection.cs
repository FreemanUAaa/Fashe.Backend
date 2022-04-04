using Fashe.Users.Core.Interfaces.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fashe.Users.Database
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connection)
        {
            services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(connection));
            services.AddTransient<IDatabaseContext, DatabaseContext>();

            return services;
        }
    }
}
