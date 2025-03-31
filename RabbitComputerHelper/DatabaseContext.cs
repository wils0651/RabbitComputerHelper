using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Computer> Computer { get; set; }
        public DbSet<ComputerTask> ComputerTask { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<UnclassifiedMessage> UnclassifiedMessage { get; set; }

        public DbSet<Probe> Probe { get; set; }
        public DbSet<ProbeData> ProbeData { get; set; }

        public DbSet<GarageDistance> GarageDistance { get; set; }
    }
}
