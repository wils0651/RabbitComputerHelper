using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Repositories.DatabaseContexts
{
    public class EventLogDatabaseContext : DbContext
    {
        public EventLogDatabaseContext(DbContextOptions<EventLogDatabaseContext> options) : base(options) { }

        public DbSet<Computer> Computer { get; set; }
        public DbSet<ComputerTask> ComputerTask { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<UnclassifiedMessage> UnclassifiedMessage { get; set; }
    }
}
