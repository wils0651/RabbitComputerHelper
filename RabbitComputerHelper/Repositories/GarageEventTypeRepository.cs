using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;
using RabbitComputerHelper.Repositories.DatabaseContexts;

namespace RabbitComputerHelper.Repositories
{
    public class GarageEventTypeRepository : IGarageEventTypeRepository
    {
        private readonly GarageSensorContext _context;

        public GarageEventTypeRepository(GarageSensorContext context)
        {
            _context = context;
        }

        public async Task<GarageEventType?> GetEventTypeByStatusIds(int previousStatusId, int currentStatusId)
        {
            return await _context.GarageEventType
                .Where(git => git.PreviousGarageStatusId == previousStatusId)
                .Where(git => git.CurrentGarageStatusId == currentStatusId)
                .FirstOrDefaultAsync();
        }
    }
}
