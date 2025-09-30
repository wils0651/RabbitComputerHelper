using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Repositories.DatabaseContexts
{
    public class GarageSensorContext : DbContext
    {
        public GarageSensorContext(DbContextOptions<GarageSensorContext> options) : base(options) { }

        public DbSet<GarageDistance> GarageDistance { get; set; }

        public DbSet<GarageStatus> GarageStatus { get; set; }

        public DbSet<GarageEventType> GarageEventType { get; set; }

        public DbSet<GarageEventLog> GarageEventLog { get; set; }
    }
}
