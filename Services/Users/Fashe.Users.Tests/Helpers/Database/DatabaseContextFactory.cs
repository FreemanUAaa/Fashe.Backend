using Fashe.Users.Core.Interfaces.Database;
using Microsoft.EntityFrameworkCore;
using Fashe.Users.Database;
using System;

namespace Fashe.Users.Tests.Helpers.Database
{
    public static class DatabaseContextFactory
    {
        public static IDatabaseContext Create()
        {
            DbContextOptions<DatabaseContext> options =
                new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            DatabaseContext context = new DatabaseContext(options);

            context.Database.EnsureCreated();

            return context;
        }

        public static void Destroy(DatabaseContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
