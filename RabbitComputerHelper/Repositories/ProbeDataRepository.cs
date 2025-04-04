using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Repositories.DatabaseContexts;

namespace RabbitComputerHelper.Repositories;

public class ProbeDataRepository : AbstractRepository, IProbeDataRepository
{
    private readonly ProbeDatabaseContext _context;

    public ProbeDataRepository(ProbeDatabaseContext context) : base(context)
    {
        _context = context;
    }
}