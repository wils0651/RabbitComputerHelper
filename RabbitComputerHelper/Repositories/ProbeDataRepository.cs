using RabbitComputerHelper.Contracts;

namespace RabbitComputerHelper.Repositories;

public class ProbeDataRepository: AbstractRepository, IProbeDataRepository
{
    private readonly DatabaseContext _context;
    
    public ProbeDataRepository(DatabaseContext context): base(context)
    {
        _context = context;
    }
}