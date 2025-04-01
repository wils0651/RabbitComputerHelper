using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Repositories.DatabaseContexts
{
    public class GarageSensorContext : DbContext
    {
        public GarageSensorContext(DbContextOptions<GarageSensorContext> options) : base(options) { }

        public DbSet<GarageDistance> GarageDistance { get; set; }
    }
}
