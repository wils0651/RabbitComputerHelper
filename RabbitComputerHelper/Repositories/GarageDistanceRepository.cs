using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;
using RabbitComputerHelper.Repositories.DatabaseContexts;

namespace RabbitComputerHelper.Repositories
{
    public class GarageDistanceRepository : AbstractRepository, IGarageDistanceRepository
    {
        private readonly GarageSensorContext _context;

        public GarageDistanceRepository(GarageSensorContext context) : base(context)
        {
            _context = context;
        }

        public async Task<GarageDistance?> GetLastWithStatusAsync()
        {
            return await _context.GarageDistance
                .Where(gd => gd.GarageStatusId.HasValue)
                .OrderBy(gd => gd.GarageStatusId)
                .FirstOrDefaultAsync();
        }
    }
}
