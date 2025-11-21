using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace StadiumAnalytics.Infrastructure.Persistence
{
    public class SqliteConfiguration
    {
        public static void Configure(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}