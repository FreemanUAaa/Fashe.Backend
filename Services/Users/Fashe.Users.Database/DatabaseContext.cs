using Fashe.Users.Core.Interfaces.Database;
using Fashe.Users.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Fashe.Users.Database
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<User> Users { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    }
}
