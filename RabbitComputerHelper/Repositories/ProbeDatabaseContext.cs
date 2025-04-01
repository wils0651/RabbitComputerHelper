using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Repositories
{
    public class ProbeDatabaseContext : DbContext
    {
        public ProbeDatabaseContext(DbContextOptions<ProbeDatabaseContext> options) : base(options) { }

        public DbSet<Probe> Probe { get; set; }
        public DbSet<ProbeData> ProbeData { get; set; }
    }
}
