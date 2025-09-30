using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;
using RabbitComputerHelper.Repositories.DatabaseContexts;

namespace RabbitComputerHelper.Repositories;

public class GarageStatusRepository : AbstractRepository, IGarageStatusRepository
{
    private readonly GarageSensorContext _context;

    public GarageStatusRepository(GarageSensorContext context) : base(context)
    {
        _context = context;
    }

    public async Task<GarageStatus?> GetStatusForDistance(decimal distance)
    {
        return await _context.GarageStatus
            .Where(g => g.MinimumDistance <= distance)
            .Where(g => g.MaximumDistance >= distance)
            .FirstOrDefaultAsync();
    }
}