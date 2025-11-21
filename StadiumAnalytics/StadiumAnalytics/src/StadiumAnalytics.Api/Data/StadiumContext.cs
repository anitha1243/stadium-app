using Microsoft.EntityFrameworkCore;
using StadiumAnalytics.Api.Models;

namespace StadiumAnalytics.Api.Data
{
    public class StadiumContext : DbContext
    {
        public StadiumContext(DbContextOptions<StadiumContext> options) : base(options) { }

        public DbSet<SensorEvent> SensorEvents { get; set; }
    }
}