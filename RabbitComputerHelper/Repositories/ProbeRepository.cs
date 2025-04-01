using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Repositories;

public class ProbeRepository : IProbeRepository
{
    private readonly ProbeDatabaseContext _context;

    public ProbeRepository(ProbeDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Probe?> GetByNameAsync(string probeName)
    {
        return await _context.Probe
            .FirstOrDefaultAsync(p => p.ProbeName == probeName.ToLower());
    }
}